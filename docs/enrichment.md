# Enrichment

The token proves **who** a principal is. It does not carry what they may do.
Enrichment is how an application resolves the second question.

## Why authorization is not in the token

A person with access to fifty contexts, each holding a handful of entitlements,
produces hundreds of entries. A browser cookie has a practical ceiling around
four kilobytes; an HTTP response body does not. Same data, impossible in one
channel and unremarkable in the other.

The failure mode when this is ignored is not an error message. It is a team
quietly granting **fewer** permissions than the business needs, because the
larger set does not fit — the architecture starts deciding the product.

There is a second reason, and it outlives the size problem. A screen that
searches across several contexts at once still has to filter the data by
context in the backend. Carrying the context list in the token would not remove
that join; it would only add a list that has to be trusted.

## The shape

1. The application validates the token **locally** — signature, expiry,
   audience. No call to the identity service on the request path.
2. It resolves the principal's authorization data from an enrichment document,
   keyed by principal.
3. It caches that document, and revalidates on its own schedule.

Step 3 is where products differ legitimately: a back-office screen and an
unattended agent do not need the same freshness. What must not differ is step 2's
*interpretation* — see [Integration](integration.md).

## Caching and revalidation

A fixed time-to-live alone leaves a question nobody can answer: *how long until
a revocation actually takes effect across the fleet?* If one application caches
for two minutes and another for eight hours, the honest answer is eight hours,
and usually nobody knows that.

Use a validator so revalidation is cheap:

```http
GET /me/entitlements
If-None-Match: "a1b2c3"

HTTP/1.1 304 Not Modified
```

A `304` carries no body. Revalidating every few minutes costs almost nothing
and bounds the revocation window to that interval, which turns "we cache for a
while" into a number you can state.

Two rules that matter more than the interval:

- **Publish the maximum.** The revocation window of the platform is the largest
  interval any consumer uses. If that number is not written down, it is not a
  guarantee.
- **Serve stale only deliberately.** When the identity service is unreachable,
  continuing with a cached document is a reasonable choice — extending it
  indefinitely is not. Decide the ceiling, and log when it is used.

## Failure

When authorization data cannot be resolved, **deny**. See [Errors](errors.md)
for why this is not negotiable and what it should look like.

## Revocation semantics

| Source | Resolved | Revocation takes effect |
| --- | --- | --- |
| Enrichment document | at each check, from cache | at the next revalidation |
| Role capabilities | at each check, from configuration | immediately |
| Machine entitlements in a token | stamped at issuance | when the token expires |

Three different answers in one platform, each defensible on its own. What
matters is knowing which one applies to the thing being revoked — the moment to
find out is not during an incident.
