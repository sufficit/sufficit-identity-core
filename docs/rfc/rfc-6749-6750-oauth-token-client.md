# RFC 6749 + RFC 6750 — OAuth 2.0 Token Response (client side) & Bearer Token Usage

**Status:** ✅ compliant — the success-response DTO reproduces the RFC 6749 §5.1/§4.2.2 wire names, optionality and omission semantics exactly (nullability `c0b4d8e`, `expires_in` and the §5.2 error shape `ec0787f`); RFC 6750 is partially not applicable (the library performs no HTTP bearer transmission).
**Last reviewed:** 2026-09-13T00:16Z · baseline `46eac0a` · fixes verified at `ec0787f`
**Project role:** OAuth **client** (token receiver). `TokenResponse` models the JSON body an OAuth client receives from the token endpoint. This library is not an authorization server and has no endpoints: `src/Sufficit.Identity.Core.csproj` references only `Microsoft.Extensions.*` and `System.Text.Json` packages (read in this review), so no ASP.NET Core/HTTP surface exists to host endpoints or read `Authorization` headers.

## What the standard requires (only requirements touching this project)

- RFC 6749 §5.1 (successful token response): `access_token` REQUIRED, `token_type` REQUIRED, `expires_in` RECOMMENDED (omittable), `refresh_token` OPTIONAL, `scope` OPTIONAL when identical to the requested scope and REQUIRED otherwise; parameters are top-level JSON members; **the client MUST ignore unrecognized value names**.
- RFC 6749 §4.2.2 (implicit grant response): the same parameter names delivered in the URI fragment (`state` REQUIRED if sent — a host-side concern, not modeled here).
- RFC 6749 §5.2 / §4.1.2.1 / §4.2.2.1: error responses (`error`, `error_description`, `error_uri`, typically HTTP 400) — a client must be able to recognize them.
- RFC 6749 §3.3: `scope` is a space-delimited, case-sensitive list; `scope-token = 1*( %x21 / %x23-5B / %x5D-7E )`.
- RFC 6749 §7.1: the client MUST NOT use an access token whose type it does not understand.
- RFC 6750 §2.1–§2.3 (three bearer transmission methods), §3 (`WWW-Authenticate`), §3.1 (`invalid_token`, `insufficient_scope`): bind parties that send or evaluate bearer tokens over HTTP — assessed under "RFC 6750 decision" below.

## Where the project complies

- `access_token` member name, non-null string (§5.1 REQUIRED) → `src/Identity/TokenResponse.cs:16-17`.
- `token_type` member name (§5.1 REQUIRED) → `src/Identity/TokenResponse.cs:25-26`.
- `expires_in` member name, JSON number type (`int`) → `src/Identity/TokenResponse.cs:22-23` (§5.1: "Numerical values are included as JSON numbers").
- `scope` stored verbatim as the single space-delimited string → `src/Identity/TokenResponse.cs:28-29` (§3.3 format preserved; no re-encoding that could break case-sensitivity).
- `refresh_token` member name → `src/Identity/TokenResponse.cs:31-32` (nullability divergence in Gaps).
- Client-side claim-bag access to the token string → `src/Identity/ClaimTypes.cs:60` (`AccessToken = "access_token"`) and `src/Identity/ClaimsPrincipalExtensions.cs:29-37` (`GetAccessToken` reads a claim only; no HTTP types are involved).
- Token supply abstraction → `src/Identity/ITokenProvider.cs:9-12` (`GetTokenAsync(): ValueTask<string?>`); the default implementation returns null → `src/Identity/AnonymousTokenProvider.cs:12-15`.

**RFC 6750 decision — partially not applicable (⚪).** Verified by a full read of all 27 files in `src/Identity/`, both files at the `src/` root, the project file, and a sample of every product folder (see absence proof below): no code reads `Authorization` headers, constructs bearer requests, or parses `WWW-Authenticate`/`invalid_token`/`insufficient_scope`. The only RFC 6750-adjacent surfaces are the claim constant + `GetAccessToken` (a host-supplied claim, not an HTTP extraction) and the opaque `TokenType` string carried by `TokenResponse`. Sections 2.1–2.3, 3 and 3.1 impose obligations on HTTP clients and resource servers; this library plays neither role in code. It is not "not applicable" outright only because `TokenResponse` is the carrier of the bearer token and `token_type: "Bearer"` values flow through it.

## Gaps

| Priority | Gap | Concrete impact | Recommendation |
|---|---|---|---|
| 🔵 | §7.1 ("MUST NOT use an access token if it does not understand the token type") is not supported by any API; `TokenType` is an opaque pass-through and no type normalization exists | Consumers get no help honoring the obligation | Document the consumer obligation on `TokenType`, or add a helper validating known types (case-insensitive per §7.1, e.g. `bearer`) |

Resolved in `c0b4d8e`: `refresh_token`/`scope` are now `string?` with `JsonIgnore(WhenWritingNull)` (`src/Identity/TokenResponse.cs:28-35`), and round-trip tests cover the §4.4.3-legal response, the full §5.1 response and wire-omission on re-serialization (`tests/Sufficit.Identity.Core.Tests/TokenResponseOptionalityTests.cs`).

Resolved in `ec0787f`: `expires_in` is now `int?` (omittable per §5.1; absence no longer collapses into an "expires immediately" `0`) and the §5.2 error shape has its own DTO, `TokenErrorResponse` (`src/Identity/TokenErrorResponse.cs`), with the robust non-null-`error` shape test documented on the type.

Searches performed for claimed absences: every `.cs` file read in full — all 27 files in `src/Identity/`, `src/PrincipalExtensions.cs`, `src/UnauthenticatedExpection.cs`, `src/Sufficit.Identity.Core.csproj`, and all 6 files in `tests/Sufficit.Identity.Core.Tests/`. Across those reads there is no occurrence of `Authorization`, `WWW-Authenticate`, `Bearer` (only a doc-comment example at `src/Identity/TokenIntrospectionResponse.cs:36`), `grant_type`, an `error` JSON member, `HttpClient`, or `HttpContext`. The remaining product folders (`src/AI`, `Cloud`, `Exchange`, `Finance`, `Gateway`, `Provisioning`, `Relacionamento`, `Sales`, `Telephony`) were all enumerated and 16 of their files read — every one is an `Entitlement`/`IRole` catalog with no transport code; the 25 unread siblings all carry `*Entitlement`/`*Role` names (absence claim for those is inferred from naming + structure, marked accordingly). At the start of this review run, `docs/rfc/` contained only `AGENTS.md` (verified by directory listing); `README.md` and other `rfc-*.md` documents appeared during the run, authored by parallel reviewers — this document makes no claims about their content.

## Intentional divergences

- `id_token` (`src/Identity/TokenResponse.cs:19-20`) is an OpenID Connect extension, not an RFC 6749 §5.1 member — deliberately added for OIDC flows; harmless under §5.1's "client MUST ignore unrecognized names" when the server does not send it.
- The library models only the response side of the token endpoint. Request bodies (§4.1.3, §4.3.2, §4.4.2, §6 `grant_type` parameters) are out of its role here — consuming applications use their own HTTP stacks; hosts must also supply §5.1's `Cache-Control: no-store`/`Pragma: no-cache` handling, which is a server obligation.
- `UserPrincipal`/`ClaimsPrincipalExtensions` role-claim handling (`src/Identity/UserPrincipal.cs:83,100-107,116-123`; the `GetRoles` method closing `src/Identity/ClaimsPrincipalExtensions.cs`) is application-level claim processing over claims the host already extracted. RFC 6749 §1.4 leaves token contents opaque to the client, so no §6749 obligation applies to it.
- `AuthorizationState` (`src/Identity/AuthorizationState.cs:13-17`) loosely evokes the §4.1.1 `state` parameter but carries an unvalidated `returnUrl`; the type performs no validation, so the open-redirect risk is a host-side responsibility (reported as an out-of-scope finding, not a library gap).

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
|---|---|---|
| 2026-09-12T23:02Z | `46eac0a` | Initial analysis (regenerate mode; no prior document existed) |
| 2026-09-12T23:55Z | `c0b4d8e` | Nullability + wire-omission fixes and round-trip tests verified; `expires_in` optionality and error model remain open |
| 2026-09-13T00:16Z | `ec0787f` | `expires_in` now `int?` (omittable per §5.1) and `TokenErrorResponse` added (§5.2); status raised to compliant |

## References

- https://www.rfc-editor.org/rfc/rfc6749 (§1.4, §3.3, §4.1.2.1, §4.2.2, §4.4.3, §5.1, §5.2, §7.1)
- https://www.rfc-editor.org/rfc/rfc6750 (§2, §3, §3.1, §4, §5)
- Sibling review: `docs/rfc/rfc-7662-token-introspection.md`
