# 0003 — Authorization data is resolved, not carried

## Decision

The access token and the session cookie carry **identity**. What a principal
may do is resolved by the application through
[enrichment](../enrichment.md) and cached locally.

## The forcing constraint

A principal with access to fifty contexts, each holding a handful of
entitlements, produces hundreds of entries. A browser cookie has a practical
ceiling around four kilobytes. An HTTP response body has none worth mentioning.

The same data is impossible in one channel and unremarkable in the other. The
problem was never the volume; it was the channel.

What makes this urgent rather than theoretical: when the set does not fit,
nothing errors. A team simply grants **fewer** permissions than the business
needs, and the architecture quietly starts deciding the product.

## The reason that outlives the size problem

Even if every entitlement fit, a screen that searches across several contexts
still has to filter its data by context in the backend. Carrying the context
list in the token would not remove that join — it would only add a list that
has to be trusted, alongside the query that has to be written anyway.

Authorization for that screen belongs in the `WHERE` clause: *rows whose
context is one where this principal holds this entitlement*. Resolved at query
time, from the database, it is both smaller and harder to get wrong.

## What was rejected

**One context at a time.** Issuing a token scoped to a single active context
bounds the size neatly and is what several large platforms do. Rejected here
because the product genuinely needs several contexts on one screen — an
administrator searching call records across every company they oversee is a
normal case, not an edge one. It would also not have solved the second problem:
a single context with enough features still overflows.

**Compressing the values.** Shorter identifiers, bitmasks, dropping hyphens
from GUIDs — all real savings, all in the single digits of percent. None of
them change the channel, which means none of them change the outcome; they just
move the cliff a little further away.

**Consulting the identity service on every request.** Rejected: it makes the
identity service a dependency of every request in the platform, and turns its
availability into everyone's availability.

## The cost accepted

Resolution is cached, so a revoked permission survives until the next
revalidation. That window is bounded and must be published — see
[Enrichment](../enrichment.md). A short validator-based revalidation keeps it
to minutes without putting the identity service on the request path.

## What would make this wrong

If the enrichment document itself grows past what an application can hold
comfortably in memory per principal, the answer is not to move it back into the
token — it is to stop returning the whole thing, and answer specific questions
instead.
