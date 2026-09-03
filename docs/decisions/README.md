# Decision records

Short notes on why the design is the way it is.

They exist because the questions they answer get asked again — by someone new,
a year later, looking at a choice that appears arbitrary. A decision recorded
with its reason is what stops a deliberate trade-off from being "fixed".

| Record | Decision |
| --- | --- |
| [0001](0001-entitlement-naming.md) | The published vocabulary is *entitlement*; the C# types keep their names for now |
| [0002](0002-no-entitlement-hierarchy.md) | Roles are hierarchical, entitlements are not |
| [0003](0003-authorization-out-of-the-token.md) | Authorization data is resolved by the application, not carried in the token |
| [0004](0004-scope-naming-and-audience.md) | Claim-releasing scopes carry no audience; API scopes are named after the API |
| [0005](0005-manifest-location.md) | Each product's provisioning manifest lives in its own repository, at the root (`identity-manifest.v1.json`) |

## Writing one

State what was decided, what was rejected, and — most importantly — **what
would make this wrong**. A record that only argues for its own conclusion is
advocacy; one that names the conditions under which it should be revisited is
useful.
