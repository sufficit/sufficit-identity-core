# Concepts

Three words get used interchangeably in most codebases and mean three different
things here. Getting them apart is most of the design.

## Scope belongs to the application

A scope says what an **OAuth client** may ask for. It is a property of the
application, not of the person using it — which is why it is what a consent
screen shows: *this application wants to read your profile*.

Scopes live in a global catalogue. They cannot be per-context, and they do not
vary between two users of the same application. That makes them the wrong tool
for deciding which button a particular person sees.

A common mistake is to reach for scopes to filter a user interface. It does not
scale: you would need one scope per feature per context, in a catalogue that is
global by definition.

## Role is a coarse grouping of a person

A role answers *what kind of participant is this person in this context?* —
administrator, operator, auditor. It drives **navigation**: which screens exist,
which links appear.

Roles are deliberately few. Their job is to make granting bearable: without
them, giving a new administrator their permissions would mean listing dozens of
individual entitlements by hand, every time.

Roles form a hierarchy — a higher role includes what a lower one grants. That
hierarchy is a graph, not a number: see [Roles](roles.md) for why a numeric
"level" is the wrong model.

## Entitlement is a specific permission in a specific context

An entitlement answers *may this person do this specific thing, here?* — save
this record, view this balance, listen to this call. It drives **actions**.

Entitlements are deliberately granular and product-specific. Each product
defines its own, because "may update a dial plan" means nothing outside
telephony. They are not hierarchical: `telephony.*` would force every product
into a shared naming scheme and take away exactly the freedom that makes them
useful.

An entitlement is always paired with a **context** — the tenant, company or
scope of data it applies to. The same person may hold different entitlements in
different contexts at the same time, and holding one in context A says nothing
about context B.

## Why the split is the way it is

Hierarchy in roles, none in entitlements, is not an accident:

- Granularity in entitlements is what lets each product speak its own language.
- Hierarchy in roles is what stops granting from becoming an endless list.

Hierarchy in both would create ambiguity about which one wins. Hierarchy in
neither would make granting unbearable. One compensates for the other.

## Where each one is decided

| Question | Answered by | Decided where |
| --- | --- | --- |
| May this application request this? | Scope | Identity, at token issuance |
| Which screens exist for this person? | Role | The application, from the enrichment document |
| May this person perform this action here? | Entitlement | The application, from the enrichment document |
| Which contexts may this person see? | Entitlement set | The backend, as a query filter |

The last row matters more than it looks. A screen that searches across several
contexts at once does not ask "which contexts are in my token" — it asks the
database to filter by the contexts where this person holds the relevant
entitlement. Authorization becomes part of the query, not a list that travelled
in a cookie.

## What is not in the token

Authorization data is not carried in the access token or the session cookie.
The token proves **who** the principal is; the application resolves **what they
may do** through [enrichment](enrichment.md).

This is not a stylistic preference. A person with access to fifty contexts,
each with a handful of entitlements, produces hundreds of entries — well past
the practical size limit of a browser cookie, and past the point where any of
it is useful for filtering a query anyway.
