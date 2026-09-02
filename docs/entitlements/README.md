# Entitlement catalogue

33 entitlements across 10 feature areas. Each product owns its own
vocabulary: an entitlement means nothing outside the feature that defines it,
which is why they are grouped this way instead of flattened into one hierarchy.

| Feature | Entitlements |
| --- | --- |
| [AI](ai.md) | 2 |
| [Cloud](cloud.md) | 1 |
| [Exchange](exchange.md) | 1 |
| [Finance](finance.md) | 12 |
| [Gateway](gateway.md) | 1 |
| [Identity](identity.md) | 1 |
| [Provisioning](provisioning.md) | 1 |
| [Relationship](relationship.md) | 2 |
| [Sales](sales.md) | 4 |
| [Telephony](telephony.md) | 8 |

## Adding one

A new entitlement needs a fresh `UniqueID`, a `Key` unique within its feature,
and a `Name` a human can read. Never reuse an identifier: it is the equality
key, so reuse silently grants the old permission to the new thing.

## Regenerating

This catalogue is generated from the type declarations:

```sh
python3 tools/generate-entitlement-catalogue.py
```

The generator fails when it cannot resolve an entitlement rather than omitting
it, so a green run means the list is complete.
