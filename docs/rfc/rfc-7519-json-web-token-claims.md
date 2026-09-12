# RFC 7519 — JSON Web Token claims (consumer side)

**Status:** ✅ compliant — registered claim names and shapes (`aud` string-or-array, NumericDate seconds) are modelled exactly; the historical private claim name `directive` is a documented, test-pinned transition divergence.
**Last reviewed:** 2026-09-12T23:02Z · commit `46eac0a`
**Project role:** claims consumer. The library never parses JWT bytes (no JOSE dependency — verified below); it reads claims from a host-built `ClaimsPrincipal` and models introspection members that mirror JWT claims (RFC 7662 §2.2).

## What the standard requires (the parts that touch this project)

- **§2 (StringOrURI, NumericDate)** — `iss`/`sub`/`aud` values are case-sensitive StringOrURI strings; `exp`/`nbf`/`iat` are NumericDates: integer seconds since 1970-01-01T00:00:00Z, ignoring leap seconds.
- **§4** — claim names within a claims set MUST be unique; parsers MUST either reject duplicates or use a lexically-last-wins JSON parser; claims that are not understood MUST be ignored.
- **§4.1.2** — `sub` is locally unique within the issuer or globally unique; case-sensitive.
- **§4.1.3** — `aud` is an array of case-sensitive strings, or a single string when there is exactly one audience.
- **§4.1.4** — `exp`: current time MUST be before `exp`; small clock-skew leeway MAY be allowed.
- **§4.1.5–§4.1.7** — `nbf`, `iat` (NumericDate), `jti` (unique, case-sensitive).
- **§4.2/§4.3** — public claim names should be registered or collision-resistant; private names are subject to collision and should be used with caution.

## Where the project complies

- Claim names pinned under their registered spellings → `src/Identity/ClaimTypes.cs:55` (`sub`), `:60` (`access_token`); introspection members `sub`/`aud`/`iss`/`jti` at `src/Identity/TokenIntrospectionResponse.cs:62-63, 72-74, 90-91, 96-97`, asserted by `tests/Sufficit.Identity.Core.Tests/WireContractTests.cs`.
- `aud` accepted as single string or array — exactly the two legal §4.1.3 shapes → `src/Identity/StringOrStringArrayJsonConverter.cs:21-49`; non-string array items are rejected, not guessed (`:38-39`).
- NumericDate as integer seconds → `long?` bindings at `src/Identity/TokenIntrospectionResponse.cs:44-57`, converted with `DateTimeOffset.FromUnixTimeSeconds` (`:104-118`); `IsExpired` implements the §4.1.4 current-before-exp check client-side (`:124`).
- Must-ignore for unknown claims: unknown entitlement keys and malformed values are logged and skipped, never fatal → `src/Identity/ClaimsPrincipalExtensions.cs:121-171`, `:173-185` (fail-closed: grant nothing).
- `sub` read case-sensitively as a `Guid` with the empty GUID as "no user" → `src/Identity/ClaimsPrincipalExtensions.cs:16-24`; self-context resolution denies when `sub` is absent (`:250-272`).
- The private-name risk is documented at the declaration site: `directive` "absent from the IANA JWT claim registry, which RFC 7519 section 4.3 advises against for private claims" → `src/Identity/ClaimTypes.cs:31-39`.

## Gaps

| Priority | Gap | Concrete impact | Recommendation |
|---|---|---|---|
| 🔵 | No clock-skew leeway on `IsExpired` (`src/Identity/TokenIntrospectionResponse.cs:124`) | A token expiring "now" flips to expired a few hundred ms early on skewed clocks; §4.1.4 allows (does not require) leeway | Document zero-leeway as deliberate, or add an optional leeway parameter |
| 🔵 | `GetUserId` falls back from `sub` to the Microsoft URI name-identifier claim (`src/Identity/ClaimsPrincipalExtensions.cs:21`) when `sub` is unparsable as a Guid | A non-Guid `sub` (legal StringOrURI) silently resolves the user from a different claim type, whose uniqueness scope is not the issuer's `sub` scope (§4.1.2) | Document the fallback, or make it opt-in |

Searches performed for claimed absences: `identitymodel|jwtsecuritytoken|tokenvalidationparameters|jsonwebtoken` in `src/**/*.cs` — no hits (no JWT parsing/validation in the library; §7.2 validation belongs to hosts).

## Intentional divergences

- `directive` (private claim name, §4.3 collision caution) is kept on purpose for consumers that still read it; `entitlements` — a name registered in the JWT claims registry via RFC 9068 §7.2.1.3 — is emitted alongside during the transition (`src/Identity/ClaimTypes.cs:39-50`, `:46`); readers accept both (`src/Identity/ClaimExtensions.cs:21-26`).

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
|---|---|---|
| 2026-09-12T23:02Z | `46eac0a` | Initial analysis |

## References

- [RFC 7519](https://www.rfc-editor.org/rfc/rfc7519) — JSON Web Token (JWT)
- Related: [rfc-9068-jwt-access-token-entitlements.md](rfc-9068-jwt-access-token-entitlements.md), [rfc-8725-jwt-bcp-token-validation.md](rfc-8725-jwt-bcp-token-validation.md), [rfc-7662-token-introspection.md](rfc-7662-token-introspection.md)
