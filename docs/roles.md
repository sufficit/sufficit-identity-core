# Roles

A role answers *what kind of participant is this person in this context?* It
drives navigation — which screens exist, which links appear — while
[entitlements](entitlements.md) drive actions.

## Shape

```csharp
public struct TelephonyRole : IRole
{
    public const string UniqueID = "63e90377-5a05-463c-a674-9071dd90817c";
    public const string NormalizedName = "telephony";

    public readonly Guid ID => Guid.Parse(UniqueID);
    public readonly string Name => "Telephony";

    string[] IRole.Filter => new[] { NormalizedName, "telefonia" };
}
```

`Filter` carries the historical spellings a role has answered to. It exists so
that renaming the display name never silently drops an existing assignment.

## Why roles exist at all

Without them, granting is unbearable. A new administrator would need dozens of
individual entitlements listed by hand, every time, in every context — and the
list would drift between people who were meant to be equivalent.

Roles are the grouping. Entitlements stay granular so each product keeps its
own vocabulary; roles stay coarse so granting stays humane. That trade is the
reason neither one is allowed to do the other's job.

## Hierarchy

A higher role includes what a lower one grants: an administrator can do what an
operator can do, without the assignment repeating every entitlement.

**Model the hierarchy as a graph, not a number.** The tempting shape is a
numeric level — `1`, `2`, `3` — and it breaks the first time the organisation
is not a straight line. An auditor is not above or below an operator; it is a
different axis. A numeric level forces every role onto one ruler and then needs
special cases to escape it, which is how "level 2 but also allowed to X" gets
born.

The shape that survives:

- each role declares the roles it **includes**;
- inclusion is resolved transitively at decision time;
- cycles are detected and rejected when the graph is loaded, not when a request
  arrives.

Resolving at decision time — rather than flattening once and storing the result
— means changing what a role includes takes effect without reissuing anything
to anyone.

## Where expansion happens

Roles expand into entitlements in the **consumer**, from reviewed
configuration. The database says who holds which role; the configuration says
what that role permits.

That split is deliberate and already used elsewhere in the platform for machine
principals. Its payoff: revoking is an update, and changing what a role permits
does not require reissuing a token to a single person.

The cost is that two consumers with different configuration can disagree about
what a role includes. That is the strongest argument for keeping the expansion
logic in this shared library rather than reimplementing it per product — see
[Integration](integration.md).

## Roles are not entitlements with a nicer name

A role never appears in an authorization check for a specific action. Code asks
"does this principal hold *this entitlement* in *this context*", never "is this
principal an administrator". The second question has no stable answer across
products, and code that asks it hard-codes an assumption about the hierarchy
that the hierarchy is free to change.
