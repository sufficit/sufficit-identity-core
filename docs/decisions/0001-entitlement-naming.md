# 0001 — The published vocabulary is *entitlement*

## Decision

Everything uses **entitlement**: documentation, wire format, API surface and the
C# types themselves. Both the `entitlements` and `directive` claims are emitted
and read during the transition.

The rename is a clean break in a new major version of the package. Consumers
migrate one at a time by taking the new version when they are ready; nothing
forces them to move on someone else's schedule.

> Superseded the original decision to defer the type rename. The reasoning
> below is kept because it explains why the rename had to be *clean* rather than
> gradual — the hazard it describes is real, and only disappears because no old
> name survives beside a new one.

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

## Why the rename had to be clean, not gradual

Entitlement discovery enumerates every type implementing `IEntitlement` through
reflection, and `HasPolicy<T>` matches by type. Those two facts make the usual
compatibility trick actively harmful:

- keeping the old name as a subclass of the new one
  would make reflection find **both**, so every listing shows the entitlement
  twice;
- an instance of the new type is not an instance of the old one, so
  a check written against the old name would start returning false — while still
  compiling, and while the type still exists.

A rename is therefore a coordinated change across the consumers, not something
that can be softened with an alias. It is worth doing; it is not worth doing
carelessly, and the type name is not what authorization depends on — `ID` and
`Key` are, and neither changes.

## What was rejected

**Renaming with compatibility subclasses.** Rejected for the reasons above:
duplicate discovery and silent type-check failures. A major version bump gives
the same freedom to migrate gradually without keeping two names alive in one
assembly.

**Renaming the wire values along with the types.** Rejected, and the attempt is
instructive: a mechanical replacement did exactly that, rewriting the literal
behind `ClaimTypes.Directive` from `directive` to `entitlement`. It compiled,
and every consumer still reading the old name would simply have stopped seeing
grants. Type names are a refactor; `ClaimTypes.Directive`, entitlement `Key`
values, `UniqueID` values and the serialised `IDDirective` property are a
contract. `WireContractTests` now pins them.

**Keeping `directive` everywhere.** Rejected: it puts a non-standard,
collision-prone name in a public contract that other systems are meant to
integrate with.

## What would make this wrong

If a consumer cannot take the new package version for reasons outside its own
schedule — a framework it cannot move off, a dependency that pins the old
assembly — then the clean break has become a fork rather than a migration. At
that point the honest answer is a compatibility package that maps old names to
new ones in a *separate* assembly, never beside the new types in this one.
