# RFC 9562 — Universally Unique IDentifiers (UUIDs)

**Status:** ✅ compliant — the library only consumes and compares UUIDs (never generates them), uses the Nil UUID as its sentinel, and compares by value so spelling cannot change a decision.
**Last reviewed:** 2026-09-12T23:02Z · commit `46eac0a`
**Project role:** consumer/comparator of UUIDs arriving as string claim values (`sub`, entitlement contexts, role/entitlement catalogue constants); hosts generate the identifiers, not this library.

## What the standard requires

- **§4** — text representation is the "hex-and-dash" string `4hexOctet-2hexOctet-2hexOctet-2hexOctet-6hexOctet`; case-insensitive; a fixed 128-bit value.
- **§4.1 / §4.2** — variant and version fields give the UUID structure (only relevant to generators).
- **§5.9** — the Nil UUID (`00000000-0000-0000-0000-000000000000`) communicates "no such value here".
- **§6.12** — UUIDs are opaque: consumers should not derive meaning from their bits.

## Where the project complies

- Identifiers are compared as `Guid` values, never as text, so case and hyphenation cannot split one identifier into two identities → `src/Identity/Utils.cs:17` (`Guid.TryParse`), equality by GUID on `src/Identity/Entitlement.cs:29-35`.
- The compact (N) and hyphenated (D) spellings of the same context are pinned as the *same* context by a conformance case → `src/Identity/EntitlementConformance.cs:91-96` ("compact context spelling resolves to the same context"), executed by `tests/Sufficit.Identity.Core.Tests/EntitlementConformanceTests.cs`.
- Nil/Guid.Empty as "no such value", matching §5.9 semantics: unparsable or absent `sub` yields `Guid.Empty` (`src/Identity/ClaimsPrincipalExtensions.cs:16-24`); an empty `IDContext` on a self-context entitlement resolves to the principal's own ID, and an empty user ID denies instead of granting (`src/Identity/ClaimsPrincipalExtensions.cs:250-272`); `IDRole == Guid.Empty` means "no role association" (`src/Identity/Entitlement.cs:13`, `src/AI/AIUserEntitlement.cs:28`).
- The library generates no UUIDs at all, so §4.1/§4.2/§6.1-§6.4 generator rules do not apply (search: `NewGuid` in `*.cs` under `src/` — no hits).
- Catalogue identifiers are fixed published constants with uniqueness enforced in tests → e.g. `src/Telephony/PhoneCallsEntitlement.cs:8-17`, `tests/Sufficit.Identity.Core.Tests/WireContractTests.cs` ("No_two_entitlements_share_an_identifier", "No_two_entitlements_share_a_key").

## Gaps

| Priority | Gap | Concrete impact | Recommendation |
|---|---|---|---|
| 🔵 | `Guid.TryParse` (any format) accepts brace `{…}`, parenthesised and hex spellings beyond the §4 hex-and-dash ABNF (`src/Identity/Utils.cs:17`; same in `GetUserId` at `src/Identity/ClaimsPrincipalExtensions.cs:16-24`); conformance pins only N and D forms | A provider emitting `{11111111-…}`-style contexts still grants; harmless internally, but the accepted wire grammar is wider than documented | Either pin the grammar with `Guid.TryParseExact(…, "N"/"D")` or document the leniency as intentional |
| 🔵 | Published constants and the catalogue use the compact 32-hex spelling (`UniqueID = "cf3c66abdb2448b68c284603540286de"`, `docs/entitlements/telephony.md` ID column), which is not the §4 hex-and-dash text form | Two spellings circulate for the same identifier; tooling that string-matches catalogue rows against hyphenated tokens misses | Keep compact as the canonical published spelling, but say so where the catalogue is introduced |

Searches performed for claimed absences: `NewGuid` in `*.cs` under `src/` (no hits — no generation); `TryParseExact` in `*.cs` under `src/` (no hits — lenient parsing everywhere).

## Intentional divergences

- Accepting both compact and hyphenated context spellings on the wire is deliberate and test-pinned (`src/Identity/EntitlementConformance.cs:91-96`); the equivalent divergence in documentation hygiene is enforced by `tests/Sufficit.Identity.Core.Tests/DocumentationLeakTests.cs` (canonicalises both spellings before comparing).
- The entitlement/role `UniqueID` constants are fixed 128-bit constants, not generated UUIDs of any version; their version nibble is incidental and no meaning is derived from it (§6.12 respected).

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
|---|---|---|
| 2026-09-12T23:02Z | `46eac0a` | Initial analysis |

## References

- [RFC 9562](https://www.rfc-editor.org/rfc/rfc9562) — Universally Unique IDentifiers (UUIDs) (obsoletes RFC 4122)
- Related: [rfc-8259-7493-json-interop.md](rfc-8259-7493-json-interop.md), [rfc-9068-jwt-access-token-entitlements.md](rfc-9068-jwt-access-token-entitlements.md)
