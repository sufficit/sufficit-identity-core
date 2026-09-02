# Entitlements

An entitlement is a specific permission held in a specific context. This
document covers what identifies one, how values are compared, and what makes a
value invalid.

For the list of entitlements that exist, see the
[catalogue](entitlements/README.md).

## What identifies an entitlement

Two stable identifiers, both declared explicitly:

```csharp
public class PhoneCallsDirective : Directive
{
    public const string UniqueID = "cf3c66ab-db24-48b6-8c28-4603540286de";

    public override Guid ID => Guid.Parse(UniqueID);
    public override string Key => "phonecalls";
    public override string Name => "phone call access";
}
```

- **`ID`** — the identity used for equality. Two entitlements are the same if
  and only if their `ID` matches.
- **`Key`** — the stable short name used in wire formats and configuration.

Neither is derived from the class name. That is deliberate: renaming a type is
a refactor, and a refactor must never change who can do what.

## Contexts

An entitlement without a context grants nothing. The context is the tenant,
company or data boundary the permission applies to, and it is always explicit
at the call site:

```csharp
principal.HasDirective<PhoneCallsDirective>(contextId)
```

A principal may hold the same entitlement in many contexts, different
entitlements in different contexts, or an entitlement in one context and
nothing in another. There is no ambient "current context" — a screen that spans
several contexts at once is a normal case.

Some entitlements are **self-context**: an empty stored context resolves to the
principal's own identifier rather than to "all contexts". Read the empty value
as *their own*, never as *any*.

## Value format

On the wire, an entitlement value is a string. The canonical form is:

```
<key>:<contextId>
```

for example `phonecalls:11111111111111111111111111111111`.

### Context identifiers are compared, not parsed

Context identifiers are GUIDs. `Guid.Parse` accepts both the hyphenated and the
compact (`N`) forms, so both round-trip correctly — but **string comparison
between the two forms fails**, and it fails silently:

```
11111111-1111-1111-1111-111111111111   ≠   11111111111111111111111111111111
```

Same context, two spellings, one comparison that returns false. The symptom is
"this user does not have access", which nobody traces back to formatting.

The rule: **parse to `Guid` and compare as `Guid`.** If a string comparison is
unavoidable, normalise to the canonical form first. Never compare the raw text
of two identifiers that arrived from different sources.

The canonical form for new values is the compact (`N`) form: 32 characters, no
hyphens, unambiguous in every .NET runtime.

## What makes a value invalid

A value is rejected — not corrected — when it contains:

- **whitespace**, because several consumers treat space-separated lists as
  multiple values. `"phonecalls:x balance:y"` would become two entitlements on
  the other side, which is exactly how an unintended permission gets in;
- **control characters**, because they travel through logs and headers;
- **more than 256 characters**, because a token is not a place for free text.

Rejection is per value: one malformed entry does not discard the valid ones
next to it.

## What an operator may grant

An operator grants **values**, never claim types. The claim type is fixed in
code.

That distinction is the whole security boundary. If the type were configurable,
an operator could write `role` or `scope` and escalate on their own; because it
is not, the worst a malformed grant can do is be rejected.

## Machine accounts

A client-credentials principal has no resource owner: `sub` is the client
identifier, per
[RFC 9068 §2.2](https://www.rfc-editor.org/rfc/rfc9068#section-2.2). Machine
entitlements are granted on the client registration and stamped into the access
token as the `entitlements` claim
([RFC 9068 §2.2.3.2](https://www.rfc-editor.org/rfc/rfc9068#section-2.2.3.2)).

This differs from how role-derived capabilities work, and the difference is
deliberate:

| | Resolved | Revocation takes effect |
| --- | --- | --- |
| Role capabilities | at each check | immediately |
| Machine entitlements | stamped at issuance | when the token expires |

Stamping avoids a lookup on every request; resolving makes revocation instant.
Neither is universally right. What matters is knowing which one you are relying
on when you revoke something in a hurry — with a one-hour token, a stamped
grant survives for up to an hour.

## Claim names

Two claim names carry the same values today:

- **`entitlements`** — the standard container, defined by
  [RFC 9068 §2.2.3.2](https://www.rfc-editor.org/rfc/rfc9068#section-2.2.3.2)
  with semantics borrowed from
  [RFC 7643 §4.1.2](https://www.rfc-editor.org/rfc/rfc7643#section-4.1.2).
- **`directive`** — the short historical name. It is not in the IANA JWT claim
  registry, and [RFC 7519 §4.3](https://www.rfc-editor.org/rfc/rfc7519#section-4.3)
  advises collision-resistant names for private claims.

Both are emitted and both are read. Consumers migrate to `entitlements` first;
`directive` is removed from both ends only after the last consumer has moved.
See [Versioning](versioning.md) for why the order cannot be reversed.
