# RFC 8725 (BCP 225) — JWT Best Current Practices: validation duties

**Status:** ⚪ partially not applicable — the library performs no JWT validation by design (it consumes a host-built principal); the integration guide now mandates the top checks (signature, expiry, audience, pinned algorithms, exact issuer, `typ` — the last three added in `c0b4d8e`); only the §3.10 SSRF note remains undocumented.
**Last reviewed:** 2026-09-12T23:55Z · baseline `46eac0a` · fixes verified at `c0b4d8e`
**Project role:** policy engine over validated claims. Everything in BCP 225 §3 that touches cryptography, headers or key handling happens in the host's token-validation stack, not here.

## What the standard requires (mapped to who does it)

- **§3.1** pin the algorithm set; the `alg` header MUST match the operation; one key, one algorithm. *(Host)*
- **§3.2** only cryptographically current algorithms; `none` only under an outer protective layer. *(Host)*
- **§3.3** validate all cryptographic operations; reject on any failure. *(Host)*
- **§3.8** validate `iss` (keys belong to that issuer) and `sub` (valid issuer-subject pair). *(Host)*
- **§3.9** when one issuer serves several audiences, tokens MUST carry `aud` and the application MUST validate it and reject on mismatch. *(Host — prescribed by this repo's docs)*
- **§3.10** do not trust received claims: sanitize `kid`, never blindly follow `jku`/`x5u` (SSRF). *(Host)*
- **§3.11/§3.12** explicit typing (`typ`) and mutually exclusive validation rules per token kind. *(Host)*
- Consumer-side hygiene the library *can* own: treat unparsable claims as no grant (fail closed).

## Where the project complies

- Audience validation is prescribed with the reasoning BCP 225 §3.9 uses → `docs/integration.md:15-23` ("An application that skips the audience check accepts tokens minted for a different service") and checklist `:86`; scope-to-audience binding → `docs/scopes.md:31`.
- Signature against published keys + expiry, locally, with no identity call on the request path → `docs/integration.md:15-23`; "no identity call on the request path" (`:20-23`) also matches the §3.10 instinct of not deriving trust from extra round-trips.
- Fail-closed claim handling (the library's own duty): malformed/unknown entitlement values grant nothing and are logged, never repaired → `src/Identity/ClaimsPrincipalExtensions.cs:121-171`, `:173-185`; conformance-pinned by `src/Identity/EntitlementConformance.cs` cases ("an unparsable context grants nothing", "an unknown entitlement key grants nothing") and `docs/errors.md:11-19` ("Fail closed").
- Audience data model supports the §3.9 check on the introspection path: `aud` string-or-array preserved → `src/Identity/TokenIntrospectionResponse.cs:72-87`.

## Gaps

| Priority | Gap | Concrete impact | Recommendation |
|---|---|---|---|
| 🔵 | §3.10 SSRF guidance (`jku`/`x5u` whitelisting) is not in the docs | Relevant the day a host starts fetching keys from header-supplied URLs | Note in integration doc if key fetching is ever host-configurable |

Resolved in `c0b4d8e`: `docs/integration.md:24-31` and checklist `:98-100` now pin the algorithm list (rejecting `alg: none`), require the exact-issuer match and restrict `typ` to `at+jwt` — closing the two 🔶 rows and the typing 🔵 row from the initial analysis.

Searches performed for claimed absences: `identitymodel|jwtsecuritytoken|tokenvalidationparameters|jsonwebtoken` in `src/**/*.cs` — no hits: the library contains no validation stack, which is why §3.1–§3.4 duties are classified host-side rather than missing here.

## Intentional divergences

- None at the library level. The division of labour itself (library trusts the principal, hosts own validation) is the documented architecture — `docs/decisions/0003-authorization-out-of-the-token.md` — and is consistent with BCP 225's "implementers of code that uses such libraries" audience clause (§1.1).

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
|---|---|---|
| 2026-09-12T23:02Z | `46eac0a` | Initial analysis |
| 2026-09-12T23:55Z | `c0b4d8e` | Algorithm pinning, exact-issuer and `typ` guidance added to `docs/integration.md`; status notes updated |

## References

- [RFC 8725](https://www.rfc-editor.org/rfc/rfc8725) — JSON Web Token Best Current Practices (BCP 225)
- Related: [rfc-9068-jwt-access-token-entitlements.md](rfc-9068-jwt-access-token-entitlements.md), [rfc-7519-json-web-token-claims.md](rfc-7519-json-web-token-claims.md), [docs/integration.md](../integration.md)
