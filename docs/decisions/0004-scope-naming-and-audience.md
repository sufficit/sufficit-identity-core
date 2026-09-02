# 0004 — Claim-releasing scopes carry no audience

## Decision

Scopes are one of two kinds, and the kind determines both the name and whether
the scope carries resources:

- **Claim-releasing** — named after the claim it releases (`roles`,
  `entitlements`), no product prefix, **no resource servers**.
- **API-access** — named `<product>.<capability>` where the product is the API
  that owns the resource, and it **must** name its resource servers.

The advertised scope list is derived from the registry. A scope registered only
in startup code does not exist.

## What was rejected

**One namespace for everything** (`identity.roles`, `identity.entitlements`).
Consistent, and wrong: the claim is not owned by the identity service any more
than a phone number is owned by the phone book. A product that later issues
these claims itself would have to keep a name that points somewhere else.

**Letting claim-releasing scopes carry resources.** This is what we actually
had, and it is worth writing down why it looked reasonable. The scope that
releases the authorization claim is requested by every user-facing login. Each
new API that wanted to read that claim was added to the scope's resource list,
because that made the API start working. The result: an ordinary web login
minted a token whose audience included several unrelated APIs. Nothing was
broken, no error was raised, and the blast radius of a stolen browser token
grew every time someone added a resource to make their integration work.

The failure mode is the giveaway — it only ever *adds* access, and adding
access is invisible. The version of the mistake that removes access gets found
in an afternoon.

**A `sufficit_` prefix.** Every scope here is Sufficit's. The prefix is a
constant, and a constant carries no information while pushing the part that
does further right.

## What would make this wrong

- **If a resource server started refusing tokens for want of an audience.**
  Today the claim-releasing scopes are read by services that identify the
  caller by other means. If one of them adopted strict audience validation,
  splitting the API access into its own scope stops being optional cleanup and
  becomes a prerequisite — do it first, in that order.
- **If a product needed to release a claim only its own API understands.**
  Then the claim is product-specific, the scope name should say so, and the
  first rule needs a third case rather than a stretch.
- **If the registry stopped being reachable at startup.** Deriving the
  advertised list from the registry assumes the registry is available when the
  document is built. A cached fallback that silently advertises a stale list
  would be worse than the hand-maintained list this replaces.

## Consequence for existing names

The rules describe the target. Names that predate them stay valid until each
consumer moves, on the transition described in [Versioning](../versioning.md):
the new name is added, both work, the old one is retired only after the last
consumer has moved. A scope is a request parameter sent by deployed clients —
renaming one is not a refactor, it is a breaking change to every application
that already asks for it.
