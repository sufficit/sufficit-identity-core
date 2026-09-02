# Integration

What a new application needs to do to make authorization decisions the same way
every other Sufficit product does.

## 1. Reference this library

```xml
<PackageReference Include="Sufficit.Identity.Core" Version="..." />
```

It targets `netstandard2.0` through `net10.0`, so the same package serves the
.NET Framework 4.8 web application and the newest services.

## 2. Authenticate

Validate the access token locally — signature against the published signing
keys, expiry, and **audience**. An application that skips the audience check
accepts tokens minted for a different service, which is the whole point of the
claim.

Nothing on the request path should call the identity service. If it does, the
identity service becomes a dependency of every request in the platform.

## 3. Resolve authorization

Ask the question with the context explicit:

```csharp
if (principal.HasDirective<PhoneCallsDirective>(contextId))
{
    // ...
}
```

There is no ambient current context. A principal may hold different permissions
in different contexts simultaneously, and a screen showing several at once is
ordinary.

For a query that spans contexts, do not enumerate permissions in the
application and pass a list to the database. Resolve the set of contexts where
the principal holds the entitlement, and let that set be part of the query
filter — authorization belongs in the `WHERE`, not in a list that travelled
from the client.

## 4. Cache with a validator

Pick a time-to-live that suits the product, revalidate with `If-None-Match`,
and publish the number. See [Enrichment](enrichment.md).

## Use the shared implementation

Each product legitimately chooses its own cache interval, its own moment to
revalidate, its own vocabulary of entitlements. None of that requires its own
*interpretation* of a grant.

Separate tokens and separate scopes already prevent one application from using
another's credentials — that risk is real and already handled. The risk that
remains is different: two applications reading the **same** enrichment document
and reaching **different** decisions. Token isolation does nothing about it,
because the data is shared even when the credentials are not.

Four ways that happens, none involving a token crossing a boundary:

- **Comparison.** One application parses context identifiers to `Guid`, another
  compares raw strings — and the hyphenated and compact spellings of the same
  identifier are not equal as text. One grants, the other denies.
- **The error path.** One treats an unreadable grant as *deny*, another logs and
  continues. Both look like they work.
- **Hierarchy.** Two consumers expanding role inclusion independently can
  disagree about whether one role contains another. Same person, same context,
  different answer depending on the screen.
- **Revocation window.** Different cache ceilings mean a revoked permission is
  gone in one product and live in another, and nobody knows the maximum.

Isolation limits the **blast radius** of a bug to one product, which is real and
valuable. A shared implementation lowers its **probability**. They add up; they
do not substitute for each other.

If a product must implement its own evaluation anyway, the minimum is a shared
set of test cases — the same inputs and expected decisions running in every
application, so divergence fails in CI instead of appearing in production.

## Checklist

- [ ] Token validated locally, including audience
- [ ] No identity call on the request path
- [ ] Context explicit at every check
- [ ] Cross-context queries filter in the database, not in memory
- [ ] Cache has a validator and a published ceiling
- [ ] Unresolvable decisions deny, and say why to the log only
- [ ] Interpretation comes from this library
