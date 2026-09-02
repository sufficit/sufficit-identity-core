# Versioning

This library is a contract between services that deploy independently. Two of
them are never updated at the same instant, so every change has to survive the
window where one side has moved and the other has not.

## The order is not symmetric

**Consumers accept the new form before producers emit it.**

Reversing that order breaks the platform for the length of the deployment
window. Following it makes the window uneventful:

1. Consumers learn to read both the old and the new form. Nothing changes yet.
2. Producers start emitting both.
3. Consumers migrate their reads to the new form.
4. Producers stop emitting the old form.
5. Consumers drop the compatibility path.

Steps 1 and 2 can sit in production indefinitely without harm. That is the
point: the migration is allowed to take as long as the slowest consumer needs.

The rename from `directive` to `entitlements` is following exactly this path —
see [the decision record](decisions/0001-entitlement-naming.md).

## What is safe to change

| Change | Safe | Why |
| --- | --- | --- |
| Add an entitlement type | yes | Nothing reads what it does not know |
| Add a feature area | yes | Same |
| Add a claim alongside an existing one | yes | Old readers ignore it |
| Rename a C# type | yes, with care | Identity is `ID` and `Key`, not the type name |
| Change an entitlement `Name` | yes | Display only |
| Change an entitlement `Key` | **no** | It is the wire identifier |
| Change an entitlement `ID` | **no** | It is the equality key |
| Reuse a retired `ID` | **never** | Silently grants the old permission to the new thing |
| Remove a claim consumers still read | **no** | Only after step 4 above |

## Renaming a type

Entitlement identity is the `ID` and the `Key`, both declared explicitly:

```csharp
public const string UniqueID = "cf3c66ab-db24-48b6-8c28-4603540286de";
public override string Key => "phonecalls";
```

Neither derives from the class name, so renaming the class is a refactor and
nothing more. It does, however, break compilation for consumers, which makes it
a coordinated change rather than a free one.

**Do not add a compatibility subclass.** Entitlement discovery enumerates every
type implementing `IEntitlement` through reflection, so an old name kept as a
subclass of the new one would be discovered as a *second, separate*
entitlement: duplicates in every listing. Worse, `HasPolicy<T>` matches by type,
and an instance of the new type is not an instance of the old one — checks
written against the old name would start returning false while appearing to
compile fine.

Rename in one coordinated pass across the consumers instead, or leave the type
name alone and change only the vocabulary that is not load-bearing.

## Deprecation

Mark, document the replacement, keep it working for at least one full release
of every consumer, and remove only after confirming nothing reads it — the
access logs of the identity service answer that question better than memory
does.
