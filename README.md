# Sufficit Identity Core

Shared contracts for authentication and authorization across every Sufficit
product. This library is the single place where the vocabulary lives, so that a
telephony screen, a finance report and an AI agent all answer the question
"may this principal do this?" the same way.

Targets `netstandard2.0` through `net10.0`, which is why it can be referenced
from the .NET Framework 4.8 web application and from the newest services alike.

## Why this is public

Security rests on the secrecy of keys, not of design — Kerckhoffs's principle,
restated by Shannon as "the enemy knows the system". The OpenID Connect
discovery document and the signing keys of the identity service are public by
necessity; an integrator, or an attacker, can already read how the protocol
works. What this documentation adds is a correct integration, not an attack
surface.

The line we hold is not *design versus no design*. It is **how it works**
(published) versus **what we run** (not published). Host names, addresses,
ports, database names, real tenant identifiers and account identifiers belong
to the operational inventory and never appear here. Every identifier in these
documents is a deliberately fake example.

## Documentation

Start with the concepts; the rest assumes that vocabulary.

| Document | What it answers |
| --- | --- |
| [Concepts](docs/concepts.md) | What a scope, a role and an entitlement each decide, and why they are not three levels of the same thing |
| [Entitlements](docs/entitlements.md) | The format, how identity and context are compared, and what makes a value invalid |
| [Entitlement catalogue](docs/entitlements/README.md) | Every entitlement, grouped by feature |
| [Roles](docs/roles.md) | Coarse grouping, hierarchy, and where expansion happens |
| [Integration](docs/integration.md) | How a new application connects, step by step |
| [Enrichment](docs/enrichment.md) | Where authorization data comes from, caching, revalidation and revocation |
| [Errors](docs/errors.md) | What to do when a decision cannot be made |
| [Versioning](docs/versioning.md) | How the contract changes without breaking consumers |
| [Decisions](docs/decisions/README.md) | Why the design is the way it is |

## The shape of an authorization decision

Three questions, three different answers:

- **Scope** — what the *application* may ask for. It belongs to the OAuth
  client, not to the person, which is why it appears on a consent screen.
- **Role** — a coarse grouping of a person inside a context. Drives navigation:
  which screens and links exist at all.
- **Entitlement** — a specific permission inside a specific context. Drives
  actions: which button is enabled, whether a save is allowed.

Roles keep granting from becoming an endless list. Entitlements keep each
product free to define its own vocabulary. The two exist precisely because one
compensates for the other.

## Reading a decision in code

```csharp
// May this principal read phone calls in this context?
if (principal.HasEntitlement<PhoneCallsEntitlement>(contextId))
{
    // ...
}
```

The context is always explicit. There is no ambient "current company": a
principal may hold different permissions in different contexts at the same
time, and a screen that shows several contexts at once is a normal case, not an
exception.

> **Naming.** The published vocabulary is *entitlement*, following
> [RFC 9068 §2.2.3.2](https://www.rfc-editor.org/rfc/rfc9068#section-2.2.3.2)
> and the SCIM semantics it borrows from
> ([RFC 7643 §4.1.2](https://www.rfc-editor.org/rfc/rfc7643#section-4.1.2)).
> The rename is complete in this library. Consumers migrate by taking the new
> major version when they are ready; see
> [the decision record](docs/decisions/0001-entitlement-naming.md).

## Contributing

Documentation and identifiers are written in English. The team works from
several places, and a shared vocabulary is worth more than a familiar one.
