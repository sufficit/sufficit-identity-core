# RFC 9068 — JWT access token profile: the `entitlements` claim

**Status:** ✅ compliant — the claim name is the registered one, the value vocabulary is free by design, and the citation defect (a non-existent §2.2.3.2) is fixed across this repository in `c0b4d8e`. The same wrong citation found in sibling repositories (sufficit-identity, sufficit-ai, sufficit-efdata) is being fixed there.
**Last reviewed:** 2026-09-12T23:55Z · baseline `46eac0a` · fixes verified at `c0b4d8e`
**Project role:** consumer of authorization claims carried in access tokens. The library reads the `entitlements` claim from a host-validated principal and interprets each value; issuing tokens and profile-conformant validation (§4) are authorization-server / host duties.

## What the standard requires (the parts that touch this project)

- **§2.2** — data structure of a JWT access token: `iss`, `exp`, `aud`, `sub`, `client_id`, `iat`, `jti` REQUIRED; `scope` SHOULD be present when the request had one (§2.2.3, format per RFC 8693 §4.2).
- **§2.2.3.1** — "Claims for Authorization Outside of Delegation Scenarios": an authorization server wanting to embed roles/groups/entitlements SHOULD use the `"groups"`, `"roles"` and `"entitlements"` attributes of the SCIM User resource schema (**RFC 7643 §4.1.2**) as claim types, encoding values per RFC 7643 guidance; explicitly: *"No specific vocabulary is provided for `roles` and `entitlements`."*
- **§7.2.1.3** — IANA registration of `entitlements` (specification documents: RFC 7643 §4.1.2 and RFC 9068 §2.2.3.1).
- **§4** — resource-server validation: `typ` MUST be `at+jwt`; `iss` exact match; `aud` MUST contain the resource server's own indicator; signature per RFC 7515 with `alg != none`; current time before `exp`.
- **§5/§6** — distinct `aud` per resource to prevent cross-JWT confusion; clients MUST NOT inspect token contents (§6).

**There is no §2.2.3.2 in RFC 9068.** Section 2.2.3 has exactly one child, 2.2.3.1 (verified against the RFC Editor text in this review). The section commonly cited for the `entitlements` claim is §2.2.3.1, with the registration at §7.2.1.3.

## Where the project complies

- Claim name `entitlements` is exactly the registered one → `src/Identity/ClaimTypes.cs:50`, pinned by `tests/Sufficit.Identity.Core.Tests/WireContractTests.cs:29-33`; SCIM attribution is stated at `src/Identity/ClaimTypes.cs:41-44`.
- Free value vocabulary (`key:context`) is legitimate — the profile mandates none → values parsed at `src/Identity/ClaimExtensions.cs:32-37` (first-colon split) and interpreted fail-closed at `src/Identity/ClaimsPrincipalExtensions.cs:121-171`.
- Both claim names (`entitlements`, historical `directive`) accepted during migration → `src/Identity/ClaimExtensions.cs:21-26`; equality tests in `tests/Sufficit.Identity.Core.Tests/EntitlementClaimTests.cs:29-56`.
- The plural form matches the SCIM attribute (multi-valued semantics): a single claim can carry a JSON array of grant strings → `src/Identity/ClaimsPrincipalExtensions.cs:132-158` (verified: array parsing).
- Host-side duties the docs already prescribe: local validation including **audience** → `docs/integration.md:15-23` and checklist `:86`; audience-per-product isolation → `docs/scopes.md:31` ("a claim-releasing scope declares no resource").

## Gaps

| Priority | Gap | Concrete impact | Recommendation |
|---|---|---|---|
Resolved in `c0b4d8e`: all eight §2.2.3.2 citations in this repository now read §2.2.3.1 (code, tests and four documents, anchors included), and `docs/integration.md:24-31` + checklist `:98-100` prescribe the `typ: at+jwt` restriction alongside the pinned algorithm list and exact-issuer match.

Searches performed for claimed absences: `identitymodel|jwtsecuritytoken|tokenvalidationparameters` in `src/**/*.cs` — no hits (the library neither issues nor validates JWT access tokens; §2 and §4 issuer/validator duties are host-side).

## Intentional divergences

- Value format `key:contextId` is a private convention — expressly allowed by §2.2.3.1 ("No specific vocabulary is provided"). The context is a UUID compared by value, not text (see [rfc-9562-uuid-identifiers.md](rfc-9562-uuid-identifiers.md)).
- The SCIM `roles` attribute is deliberately NOT used as the role claim; the platform uses the single-valued `role` claim (OpenID Connect style), repeated or JSON-array encoded → `src/Identity/UserPrincipal.cs:85-123`. Legal (§2.2.3.1 is a SHOULD for servers choosing that representation), documented here so it is not re-filed as a bug.

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
|---|---|---|
| 2026-09-12T23:02Z | `46eac0a` | Initial analysis — found the §2.2.3.2 citation defect |
| 2026-09-12T23:55Z | `c0b4d8e` | §2.2.3.1 citations fixed in all 8 places; `typ` guidance added; status raised to compliant |

## References

- [RFC 9068](https://www.rfc-editor.org/rfc/rfc9068) — JWT Profile for OAuth 2.0 Access Tokens
- [RFC 7643 §4.1.2](https://www.rfc-editor.org/rfc/rfc7643#section-4.1.2) — SCIM Core Schema, User resource
- Related: [rfc-7519-json-web-token-claims.md](rfc-7519-json-web-token-claims.md), [rfc-8725-jwt-bcp-token-validation.md](rfc-8725-jwt-bcp-token-validation.md)
