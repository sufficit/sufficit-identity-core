# Scopes

A scope is what an *application* may ask for. It belongs to the OAuth client,
not to the person — which is why it appears on a consent screen and why two
users of the same application always present the same scopes.

For what a scope decides compared to a role or an entitlement, see
[Concepts](concepts.md). This document covers naming, audience, and the scope
each product owns.

## Two kinds of scope, two naming rules

The distinction matters because the two do different jobs, and mixing them is
how a login token ends up accepted by an API it was never meant to reach.

### Claim-releasing scopes

They release a claim into the token. They grant no API access on their own.

| Scope | Releases |
| --- | --- |
| `openid`, `profile`, `email`, `address` | Standard OpenID Connect claims |
| `offline_access` | A refresh token |
| `roles` | The `role` claim |
| `entitlements` | The `entitlements` claim (see [Versioning](versioning.md) for the transition from the historical name) |

**Rule: the scope is named after the claim it releases.** `roles` releases
`role`; `entitlements` releases `entitlements`. No product prefix, because the
claim is not owned by a product.

**Rule: a claim-releasing scope declares no resource.** It hands out
information about the principal; it does not authorize a call. Giving it an
audience is what turns "let this app read who I am" into "let this app call
that API".

### API-access scopes

They authorize calls against a specific API.

**Rule: `<product>.<capability>`, lowercase, dotted.** The product is the API
that owns the resource, not the caller. `provisioning.manage` is owned by the
provisioning service whether the caller is a web console or a script.

**Rule: an API-access scope names its resource servers.** That is what puts an
`aud` in the token ([RFC 9068 §3](https://www.rfc-editor.org/rfc/rfc9068#section-3),
[RFC 8707](https://www.rfc-editor.org/rfc/rfc8707)). A scope with no resource
mints a token with no audience, and a token with no audience is only refused by
resource servers that bother to check — which is not a property you can verify
from the issuer's side.

No `sufficit_` prefix. Everything here is Sufficit; the prefix carries no
information and pushes the part that does carry information further right.

## Scope by product

| Product | Scopes it owns | Notes |
| --- | --- | --- |
| Identity | `identity.management`, `identity.mcp`, `identity.scim`, `identity.tokens` | Administration, MCP surface, SCIM provisioning, personal token management |
| Provisioning | `provisioning.manage`, `provisioning.installation` | Management API; single device self-registration |
| AI | `ai.user`, `ai.bridge` | Acting as a user of the assistant; the model bridge |
| Endpoints | `endpoints.invoices` | Platform API surfaces granted individually |
| Background | `background.jobs` | Scheduled and queued work |
| Network control | `network.control` | |

Applications additionally request the claim-releasing scopes they need. A web
console typically asks for `openid profile roles entitlements offline_access`
plus the API scopes of the services it calls.

## The registry is the contract, not the code

A scope exists when it has a row in the identity scope registry. Two things
follow, and both have bitten us:

1. **A scope registered only in application startup code works but is
   invisible.** It never appears in the discovery document, so an integrator
   reading `scopes_supported` concludes it does not exist and requests
   something else. It also has no row to carry resources, so it has no
   audience.
2. **A scope in the registry that startup code does not know still works**,
   because the issuer validates against the registry too. The result is the
   mirror image: it functions, nobody can discover it, and its absence from
   the advertised list looks deliberate.

The rule: **every scope has a registry row, and the advertised list is derived
from the registry.** A hand-maintained list beside a registry is two sources of
truth that agree only until someone is in a hurry.

### Where the advertised list comes from today

Not from the registry. It is assembled at startup from three places: the
standard OpenID Connect scopes, the values of the claim-to-scope map, and the
scope that gates the MCP surface. The registry is consulted when a request is
validated, and never when the document is published — which is exactly how a
scope ends up working and being undiscoverable at the same time.

Closing that gap is a code change, listed here because reading this document
and the discovery document side by side is otherwise confusing.

## Adding a scope, or renaming one

A scope name is a request parameter sent by deployed clients, so introducing
one is a four-step sequence and the order is not negotiable:

1. **Register** the new name, mirroring the resources of the name it succeeds.
   Requestable, grantable, and behaviourally identical.
2. **Grant** it to the clients that hold the old one. Nothing changes yet: a
   client only sends the scopes it is configured to send.
3. **Teach every consumer to accept it.** A resource server that checks for the
   old name rejects the new one, and a claim-releasing scope releases nothing
   until the claim-to-scope map names it. This is the step that is easy to skip
   because steps 1 and 2 look like progress.
4. **Switch the clients**, one at a time, then retire the old name once nothing
   asks for it.

Between steps 1 and 3 the new name exists and does not work. Say so in the
scope description — the registry is where someone looks before they migrate.

The manifest that carries the registry rows lives at the product's
repository root, `identity-manifest.v1.json` — see
[0005](decisions/0005-manifest-location.md) for why it is not centralized.

## What a scope must never be used for

A scope does not carry a context. `provisioning.manage` says the application
may call the provisioning management API; it does not say *for which company*.
The context comes from the entitlement, per request.

Authorizing on scope alone is how an application ends up letting an operator of
one company manage another's devices. Where both exist, the scope is the outer
gate and the entitlement is the decision — see
[Concepts](concepts.md).
