# RFC 8259 + RFC 7493 — JSON and the I-JSON message format

**Status:** 🟡 partial — the wire contracts are clean I-JSON and the convenience `DateTime` helpers now carry `Kind=Utc` (fixed in `c0b4d8e`); duplicate member names are still accepted silently on read.
**Last reviewed:** 2026-09-12T23:55Z · baseline `46eac0a` · fixes verified at `c0b4d8e`
**Project role:** producer and consumer of JSON DTOs (`TokenResponse`, `TokenIntrospectionResponse`, `AuthorizationState`) that cross service boundaries in host applications; the library never runs an HTTP stack itself.

## What the standard requires

Only the requirements that touch a DTO library:

- **RFC 8259 §8.1** — JSON text exchanged between systems MUST be encoded in UTF-8; §2 forbids unescaped control characters in strings.
- **RFC 7493 §2.1** — I-JSON messages MUST be UTF-8; strings MUST NOT contain unpaired surrogates or noncharacters.
- **RFC 7493 §2.2** — numbers SHOULD NOT exceed IEEE 754 double precision (±2⁵³−1 for exact integers); larger exact values belong in strings.
- **RFC 7493 §2.3** — objects MUST NOT have duplicate member names; receivers MAY treat such messages as invalid (§3).
- **RFC 7493 §4.2** — Must-Ignore: unrecognized members MUST NOT be treated as an error.
- **RFC 7493 §4.3** (RECOMMENDED) — time data as RFC 3339 strings with explicit timezone; the timezone be included, not defaulted.

## Where the project complies

- Wire member names are pinned snake_case, independent of C# property names → `src/Identity/TokenResponse.cs:16-32`, `src/Identity/TokenIntrospectionResponse.cs:14-97`, `src/Identity/AuthorizationState.cs:13-16`, asserted by `tests/Sufficit.Identity.Core.Tests/WireContractTests.cs:22-38` (verified in this review).
- Numeric timestamps travel as JSON numbers well inside the 2⁵³ exact-integer range (Unix seconds ≈ 10⁹) → `long?` on `src/Identity/TokenIntrospectionResponse.cs:44-57` (RFC 7493 §2.2 safe).
- Unknown JSON members are ignored, not errors: deserialization uses default `JsonSerializer` options with no `JsonExtensionData` and no strict-mode flags → `src/Identity/ClaimsPrincipalExtensions.cs:140`, `src/Identity/UserPrincipal.cs:137` (RFC 7493 §4.2).
- Application-layer Must-Ignore for unknown entitlement keys: fail-closed skip with a warning log → `src/Identity/ClaimsPrincipalExtensions.cs:173-185` (`TryParseUserPolicy`), `src/Utils` counterpart raising on unknown key at `src/Identity/Utils.cs:19`.
- The tolerant `aud` converter reads `string`, `string[]`, or `null` and rejects non-string array items with `JsonException` instead of guessing → `src/Identity/StringOrStringArrayJsonConverter.cs:21-49`.
- UTF-8 and string hygiene are delegated to `System.Text.Json` (package versions pinned per TFM at `src/Sufficit.Identity.Core.csproj:100-101`); no other JSON stack exists in `src/` (search: `newtonsoft|jsonconvert\.|jobject|jarray` in `*.cs` under `src/` — no hits).

## Gaps

| Priority | Gap | Concrete impact | Recommendation |
|---|---|---|---|
| 🔵 | Duplicate JSON member names are accepted silently on read (RFC 7493 §2.3 forbids senders; .NET POCO deserialization is documented last-wins — inferred from documented behavior, not executed here) | A hostile or buggy token source could emit `{"active":true,...,"active":false}` and the outcome depends on serializer internals | Acceptable for an internal library; document the reliance on last-wins if these DTOs ever front an untrusted boundary |
| 🔵 | `UserPolicyBase` carries `[DataContract]/[DataMember(Name=...)]` legacy names (`identitlement`, `idcontext`) alongside JSON usage (`src/Identity/UserPolicyBase.cs:8-25`) | Two serialization stacks can emit two different member names for the same field; today only the JSON path is pinned by tests | State in XML docs that the DataContract names are legacy-only, or drop the attributes in the next major |

Resolved in `c0b4d8e`: the introspection `DateTime` helpers now return `.UtcDateTime` (`Kind=Utc`) instead of unspecified-local values (`src/Identity/TokenIntrospectionResponse.cs:103-119`).

Searches performed for claimed absences: `base64|dataurl` in `*.cs` under `src/` (no hits — no binary-data members, RFC 7493 §4.4 not applicable); `newtonsoft|jsonconvert\.|jobject|jarray` in `*.cs` under `src/` (no hits).

## Intentional divergences

- `aud` accepted as `string` **or** `string[]`, and a one-element array is written back as a single string — deliberate Duende/provider interop, documented in the converter rationale (`src/Identity/StringOrStringArrayJsonConverter.cs:6-14`, write path `:55-73`). Legal under RFC 7519 §4.1.3; noted here so future reviews do not file it as a bug.
- JSON-object entitlement claim values are skipped, not rejected (`src/Identity/ClaimsPrincipalExtensions.cs:161-163`) — fail-closed Must-Ignore at the claim layer.

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
|---|---|---|
| 2026-09-12T23:02Z | `46eac0a` | Initial analysis |
| 2026-09-12T23:55Z | `c0b4d8e` | Introspection DateTime helpers fixed to `Kind=Utc`; duplicate-members and DataContract items remain open |

## References

- [RFC 8259](https://www.rfc-editor.org/rfc/rfc8259) — The JavaScript Object Notation (JSON) Data Interchange Format
- [RFC 7493](https://www.rfc-editor.org/rfc/rfc7493) — The I-JSON Message Format
- Related: [rfc-7519-json-web-token-claims.md](rfc-7519-json-web-token-claims.md) (NumericDate semantics), [rfc-9562-uuid-identifiers.md](rfc-9562-uuid-identifiers.md)
