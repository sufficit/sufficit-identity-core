# 0002 — Roles are hierarchical, entitlements are not

## Decision

A role may include other roles, resolved transitively at decision time.
Entitlements are flat: there is no `telephony.*` that implies the entitlements
beneath it.

## Why the asymmetry

The two rules solve opposite problems, and each one covers the other's cost.

**Entitlements stay flat so each product keeps its own vocabulary.** "May update
a dial plan" means nothing outside telephony. A hierarchy would require a shared
naming scheme across every product, which is exactly the coupling that makes a
new product wait on a convention discussion before it can express its own
permissions.

**Roles are hierarchical so granting stays humane.** Flat entitlements alone
would mean listing dozens of individual grants for every new administrator, in
every context, and watching that list drift between people who were meant to be
equivalent.

Hierarchy in both would create ambiguity about which one wins when a role
implies one thing and a wildcard implies another. Hierarchy in neither would
make granting unbearable. One compensates for the other, which is why the split
is where it is.

## Hierarchy as a graph, not a level

A numeric level is the tempting shape and the wrong one. It presumes a total
order, and real organisations are not linear: an auditor is neither above nor
below an operator — it is a different axis. Forcing them onto one ruler
produces special cases like "level 2, but also allowed to X", and those special
cases are where the model stops being a model.

Roles therefore declare the roles they include; inclusion is transitive; cycles
are rejected when the graph loads rather than when a request arrives.

## What was rejected

**Wildcards in entitlements** (`telephony.*`). They would collapse the common
case and reduce the number of grants — the attraction is real. Rejected because
the naming scheme becomes a cross-product contract, and because a wildcard
grants permissions that do not exist yet: an entitlement added next year is
silently included in a grant written today.

**Numeric role levels.** Rejected as described above.

## What would make this wrong

If granting through roles turns out not to cover the common cases — if
day-to-day work means assigning long lists of individual entitlements anyway —
then the role layer is not doing its job, and the answer is better roles, not a
hierarchy bolted onto entitlements.
