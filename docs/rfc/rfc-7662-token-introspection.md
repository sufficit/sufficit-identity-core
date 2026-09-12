# RFC 7662 — OAuth 2.0 Token Introspection

**Status:** 🟡 partial — the response model covers every §2.2 member with correct types and a robust string-or-array `aud` converter, but the request model omits `token_type_hint`, does not address the §2.1 form-encoding contract, and malformed/401 responses collapse into `active=false`.
**Last reviewed:** 2026-09-12T23:02Z · commit `46eac0a`
**Project role:** introspection **client** (the protected-resource side that queries an authorization server and reads §2.2 responses). `TokenIntrospectionResponse` models the JSON response; `TokenValidationRequest` models the request body. No endpoint is implemented and no HTTP call is performed inside the library: `src/Sufficit.Identity.Core.csproj` references only `Microsoft.Extensions.*` and `System.Text.Json` packages (read in this review), so the TLS (§2) and endpoint-authorization (§2.1, §4) obligations fall entirely on the consuming host.

## What the standard requires

- §2.1: the introspection request is an HTTP POST with `application/x-www-form-urlencoded` parameters; `token` REQUIRED; `token_type_hint` OPTIONAL (the server MUST extend its search across all supported token types if the hint is missing or unhelpful); the endpoint MUST require some form of authorization (client authentication or a bearer token) and MUST be TLS-protected (§2, §4).
- §2.2 response members: `active` REQUIRED boolean; `scope`, `client_id`, `username`, `token_type`, `exp`, `iat`, `nbf`, `sub`, `aud`, `iss`, `jti` OPTIONAL; `aud` is "a service-specific string identifier **or list of string identifiers**"; `exp`/`iat`/`nbf` are "integer timestamp, measured in the number of seconds since January 1 1970 UTC"; `scope` uses the §3.3 space-delimited format of RFC 6749; implementations MAY extend with service-specific members; inactive tokens SHOULD carry only `active: false`.
- §2.3: invalid client credentials or an insufficient authorization token yield HTTP 401 — an error, not an `active:false` response.

## Where the project complies

- `active` REQUIRED boolean → `src/Identity/TokenIntrospectionResponse.cs:14-15`.
- OPTIONAL string members with correct wire names → `scope` `:20-21`, `client_id` `:26-27`, `username` `:32-33`, `token_type` `:38-39`, `sub` `:62-63`, `iss` `:90-91`, `jti` `:96-97` (all nullable, matching §2.2 optionality).
- `exp`/`iat`/`nbf` as nullable 64-bit Unix-seconds → `:44-45`, `:50-51`, `:56-57`; UTC epoch conversion via `DateTimeOffset.FromUnixTimeSeconds` → `:103-105`, `:110-112`, `:117-119`. The zero-offset `.DateTime` carries UTC wall time, and `IsExpired` (`:124`) compares ticks against `DateTime.UtcNow`, so the comparison is correct despite `Kind=Unspecified` (inferred from documented .NET semantics; not executed).
- `aud` string-or-array tolerance → `src/Identity/StringOrStringArrayJsonConverter.cs`: JSON `null` → empty array (`:21-22`), single string accepted (`:24-26`), array accepted with null elements skipped (`:28-46`), a non-string array element raises `JsonException` (`:38-39`); the write side emits a bare string for single-element arrays (`:61-65`), covering both shapes §2.2 allows.
- `aud` modeled as `string[]` defaulting to empty → `src/Identity/TokenIntrospectionResponse.cs:72-74` — an absent OPTIONAL member degrades to "no audience" instead of a null trap.
- Space-delimited `scope` parsing (§2.2 → RFC 6749 §3.3) → `Scopes` splits on `' '` with `RemoveEmptyEntries` → `src/Identity/TokenIntrospectionResponse.cs:129-131`.
- Extension tolerance: unknown service-specific members are ignored by System.Text.Json defaults (inferred — documented STJ behavior; there is no `[JsonExtensionData]` capture, so extensions are dropped, which still satisfies §2.2's MAY).
- The REQUIRED request parameter exists → `src/Identity/TokenValidationRequest.cs:6-11` (`Token`, initialized to `string.Empty`).

## Gaps

| Priority | Gap | Concrete impact | Recommendation |
|---|---|---|---|
| 🔶 | `TokenValidationRequest` represents only `{ token }` and does not address the §2.1 `application/x-www-form-urlencoded` POST contract (`src/Identity/TokenValidationRequest.cs:6-11`) | Consumers must hand-build the form body; nothing steers them away from JSON-POSTing the DTO, which conformant servers reject or ignore | Document the wire mapping next to the type, or add a helper that produces `token=<value>` form content |
| 🔵 | `token_type_hint` (§2.1 OPTIONAL) is absent from the request model | No correctness impact — the hint is optional and the server MUST search all token types without it — but busy authorization servers lose the lookup optimization | Add `string? TokenTypeHint` |
| 🔵 | `active` is a non-nullable `bool` (`:14-15`) and no §2.3 error shape exists | An HTTP 401 body or a spec-violating response without `active` deserializes to `active=false` — fail-closed, but a consumer cannot distinguish "token inactive" from "call failed / misconfigured" | Keep the DTO simple but document that callers must check the HTTP status before trusting `active`; consider `bool?` |
| 🔵 | No tests cover `TokenIntrospectionResponse`, `TokenValidationRequest`, or the converter (all 6 files in `tests/Sufficit.Identity.Core.Tests/` read; none references them) | The `aud` dual-shape behavior and the Unix-second semantics can regress unnoticed | Round-trip tests: string `aud`, array `aud`, null `aud`, and `exp` → `IsExpired` |

Searches performed for claimed absences: all 27 files under `src/Identity/` were read in full — no occurrence of `token_type_hint`, `FormUrlEncoded`, `HttpClient`, `MemoryCache`, or `Cache-Control`; no introspection route/endpoint exists (no ASP.NET Core packages in `src/Sufficit.Identity.Core.csproj`, verified by reading it). All 6 test files under `tests/Sufficit.Identity.Core.Tests/` were read — none references `TokenIntrospectionResponse`, `TokenValidationRequest`, or `StringOrStringArrayJsonConverter`.

## Intentional divergences

- `Audiences` collapses to its first element through the `Audience` convenience property (`:80-85`) — documented in-code as a backward-compatible shortcut for legacy callers; multi-audience consumers must use `Audiences`.
- The client DTO does not model §2.2's caching allowance or §4's "MUST NOT be cached beyond `exp`" rule — those bind protected-resource/AS infrastructure. `ExpirationDateTime` and `IsExpired` expose exactly what a correct cache policy needs.
- RFC 8693 (token exchange) request/response modeling does not exist in this library; within this review's domain that only requires the space-delimited `scope` handling above, which is compliant. The absence was confirmed by the full read of `src/Identity/` (no `subject_token`, no `exchange` patterns).

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
|---|---|---|
| 2026-09-12T23:02Z | `46eac0a` | Initial analysis (regenerate mode; no prior document existed) |

## References

- https://www.rfc-editor.org/rfc/rfc7662 (§1, §2, §2.1, §2.2, §2.3, §4, §5)
- https://www.rfc-editor.org/rfc/rfc6749 (§3.3 scope syntax, referenced by §2.2)
- Sibling review: `docs/rfc/rfc-6749-6750-oauth-token-client.md`
