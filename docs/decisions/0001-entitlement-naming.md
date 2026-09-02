# 0001 — The published vocabulary is *entitlement*

## Decision

Documentation, wire format and new API surface use **entitlement**. The 33
existing C# types keep their `*Directive` names for now, and both the
`entitlements` and `directive` claims are emitted and read during the
transition.

## Why *entitlement*

`entitlements` is the container defined by
[RFC 9068 §2.2.3.2](https://www.rfc-editor.org/rfc/rfc9068#section-2.2.3.2),
with semantics borrowed from
[RFC 7643 §4.1.2](https://www.rfc-editor.org/rfc/rfc7643#section-4.1.2).
Carrying this kind of grant in an access token is the path the standard already
describes, not an invention that needs defending.

`directive` is a short name with no namespace and no entry in the IANA JWT
claim registry.
[RFC 7519 §4.3](https://www.rfc-editor.org/rfc/rfc7519#section-4.3) advises
collision-resistant names for private claims precisely to avoid the day a
second system means something else by the same word.

## Why the types are not renamed yet

Entitlement discovery enumerates every type implementing `IDirective` through
reflection, and `HasPolicy<T>` matches by type. Those two facts make the usual
compatibility trick actively harmful:

- keeping `PhoneCallsDirective` as a subclass of a new `PhoneCallsEntitlement`
  would make reflection find **both**, so every listing shows the entitlement
  twice;
- an instance of the new type is not an instance of the old one, so
  `HasPolicy<PhoneCallsDirective>` would start returning false — while still
  compiling, and while the type still exists.

A rename is therefore a coordinated change across the consumers, not something
that can be softened with an alias. It is worth doing; it is not worth doing
carelessly, and the type name is not what authorization depends on — `ID` and
`Key` are, and neither changes.

## What was rejected

**Renaming with compatibility subclasses.** Rejected for the reasons above:
duplicate discovery and silent type-check failures.

**Renaming everything at once, across all consumers.** Not rejected on merit —
deferred. It is a mechanical change that touches three repositories and should
land on its own, not folded into a documentation pass.

**Keeping `directive` everywhere.** Rejected: it puts a non-standard,
collision-prone name in a public contract that other systems are meant to
integrate with.

## What would make this wrong

If a fourth consumer appears and starts writing `directive` into new code, the
transition has stalled and the cost of the rename is growing rather than
shrinking. At that point, do the coordinated pass instead of extending the
compatibility window again.
