# RFC review

How this library maps to the Internet standards its code actually touches —
and where it deliberately does not. Each document pins the reviewed commit,
cites evidence as `path:line`, and separates gaps from intentional
divergences.

**Last reviewed:** 2026-09-12T23:02Z · commit `46eac0a` · scope: `src/` of
this repository (a class library; hosts own the HTTP stack).

## Applicable standards

| Standard | Status | One line |
| --- | --- | --- |
| [RFC 6749 + 6750](rfc-6749-6750-oauth-token-client.md) — OAuth 2.0 / Bearer | 🟡 partial | `TokenResponse` mirrors §5.1, but `refresh_token`/`scope` are non-nullable although the spec makes them OPTIONAL/conditional |
| [RFC 7662](rfc-7662-token-introspection.md) — Token Introspection | ✅ compliant | Response model is §2.2 member-for-member, `aud` string-or-list included; advisory: add `token_type_hint`, warn about inactive-token members |
| [RFC 7519](rfc-7519-json-web-token-claims.md) — JWT claims | ✅ compliant | Registered names and shapes modelled exactly; `directive` private name is a documented, test-pinned transition divergence |
| [RFC 9068](rfc-9068-jwt-access-token-entitlements.md) — JWT access token profile | 🟡 partial | 🔴 The claim name is right but the repo pins a **non-existent section (§2.2.3.2)**; correct citation is §2.2.3.1 (registered §7.2.1.3) |
| [RFC 8725](rfc-8725-jwt-bcp-token-validation.md) — JWT BCP 225 | ⚪ partially N/A | Library validates nothing by design; the three top checks are already prescribed — pinning the algorithm set and issuer match are the missing lines |
| [RFC 8259 + 7493](rfc-8259-7493-json-interop.md) — JSON / I-JSON | 🟡 partial | Wire contracts are clean I-JSON; convenience `DateTime` properties drop the UTC marker; duplicate members silently accepted |
| [RFC 9562](rfc-9562-uuid-identifiers.md) — UUIDs | ✅ compliant | Compare-by-value so spelling cannot split an identity; Nil UUID as sentinel; accepted grammar is wider than the §4 ABNF (documented) |

The one 🔴 finding of this review — the wrong RFC 9068 section number pinned
in code, tests and four documents — is detailed in the
[RFC 9068](rfc-9068-jwt-access-token-entitlements.md) document, with all
seven affected locations.

## Judged not applicable, and why

| Family | Why not applicable |
| --- | --- |
| HTTP semantics & edge (9110–9112, 6585, 9457, 8288, 5789, 7240, 9205, idempotency, rate limits, 9745/8594) | No HTTP stack exists here: `httpclient\|httpwebrequest\|webclient\|restsharp\|system.net.http\|socket\|websocket\|grpc` in `src/**/*.cs` — zero hits |
| CORS, HSTS, proxies (6797, 7239, 8305, 8446, 6455, 9116) | `addcors\|usecors\|hsts\|forwardedheaders` in `src/**/*.cs` — zero hits |
| OAuth flows & server duties (7636, 8628, 8414, 9728, 8707, 7591, 7009, 9700, 9449, 8693, OIDC) | The library implements no endpoint, registration or flow; hosts play those roles |
| Observability (W3C Trace Context, OpenTelemetry) | `traceparent\|correlation\|activity\.\|opentelemetry` in `src/**/*.cs` — zero hits |
| SSE, JSON-RPC, MCP, OpenAPI, webhooks | Absent from the codebase; not part of this library's surface |
| RFC 1918 (private addresses) | Appears only as a documentation-hygiene rule enforced by `DocumentationLeakTests`, not as a runtime concern |
| RFC 9111 `If-None-Match` | Referenced as consumer guidance in `docs/enrichment.md`; no HTTP client here to implement it |

## Method and rules

Written under the rules in [AGENTS.md](AGENTS.md): zero prior knowledge (every
claim re-verified against code opened in the same run), citations only for
lines actually read, absences proven by stated searches, verified vs inferred
marked, no secrets. Priorities: 🔴 security/data loss/broken clients, 🔶
interoperability/reliability, 🔵 polish.

## Revision history

| Reviewed (UTC) | Commit | Summary of changes |
| --- | --- | --- |
| 2026-09-12T23:02Z | `46eac0a` | Initial review: 7 applicable standards, families excluded with evidence |
