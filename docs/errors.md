# Errors

Authorization has an asymmetry that shapes every decision in this document:
**granting too much produces a working screen.** Nobody reports being able to
see more than they should. Denying too much produces a support ticket within
the hour.

So the errors that survive in production are the permissive ones, and the code
has to be written knowing that.

## Fail closed

When a decision cannot be made — malformed value, unreachable service, expired
cache past its ceiling, unparsable context — the answer is **deny**.

This library already does that. `GetUserPolicies` logs and skips a claim it
cannot read, so one product-specific malformed value cannot reject a principal
outright; and the AI access check treats any parse failure as *no contexts
assigned* rather than continuing. Both are the same instinct: an unreadable
grant is not a grant.

## Say why, to the log

Failing closed silently is how a formatting mistake becomes a half-day of
debugging. The symptom — "this user has no access" — points at permissions,
which is the one place the problem is not.

Log enough to distinguish the cases:

- the value was **absent** (no grant exists);
- the value was **present and rejected** (grant exists, and is malformed);
- the source was **unavailable** (grant unknown, serving stale or denying).

Those three are indistinguishable to the user and completely different to
whoever is called at night.

## Say nothing useful to the caller

The response should not reveal whether a resource exists, whether a context is
real, or which entitlement was missing. Return the same answer for "not yours"
and "does not exist": distinguishing them confirms that an identifier is valid,
which is a free reconnaissance step.

That is a deliberate trade against debuggability, and it is why the previous
section exists — the detail goes to the log, not to the response.

## Never repair a value

A malformed entitlement is rejected, not trimmed, normalised or guessed. If a
value arrives with a space in it, the space came from somewhere, and inventing
the operator's intent is how one grant silently becomes two.

Rejection is per value: one bad entry does not discard the valid ones beside
it, because that would turn a typo into a lockout.

## Do not cache a failure as an answer

Caching "denied" from a failed lookup turns a transient outage into a lasting
one, long after the service came back. Cache decisions derived from data you
actually received; on failure, either serve the previous document within its
ceiling, or deny without recording that denial.
