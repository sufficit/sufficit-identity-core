# Commercial integration authorization

| Grant | Stable ID | Rights |
| --- | --- | --- |
| salesintegrationaudit | e578b7d796924f09846955f619bb45aa | Read audit metadata in assigned contexts |
| salesintegrationretry | b928117604374e47b3630fdbe163bc2a | Request bounded transport retries in assigned contexts |

Neither grant derives a role. Read does not imply retry; retry does not imply read, financial access, telephony control or platform administration. A normal existing administrator retains access. Query/command contexts must be explicit nonempty GUIDs. Existing policy semantics allow an explicitly assigned Guid.Empty grant across all nonempty contexts; this is an intentional global grant, not the default.

API boundaries inspect the context independently. For InternalPermissionUser, trusted InternalPermissions is authoritative even if incoming tokens contained business claims or administrator roles. Internal scope filtering and revocation snapshots cannot be bypassed by normal claim fallback. Permission freshness follows the existing bounded internal cache lifetime, not immediate global invalidation.

GET Sales/IntegrationAudit/Permissions?contextId=... returns SalesIntegrationAccessState (ContextId, CanRead, CanRetry) without reading events, customer existence or private data. Retry also requires an authenticated GUID requester. This response is no-store, not an authorization credential: both event reads and commands reauthorize. The Blazor page uses the API decision, including internal grants missing from browser claims. Authentication identity changes clear pending UI state; late command receipts cannot appear in the new identity.

Internal token-scope mappings must explicitly include the new keys when delegating through that path. Example desired mappings: salesintegrationaudit => sales.integration.audit.read; salesintegrationretry => sales.integration.retry. Issuer/client scope approval and existing user policy assignment remain required. Do not add users, client IDs or scope mappings globally just to enable a screen. These binaries introduce the capability but assign no real customer/user grants.

Rollout: update Identity.Core, Client and all APIs before the Blazor permission probe consumer. Preserve existing retry/capture/delivery flags; no schema change required. Rights assignment consumers must load the updated Identity.Core registry before presenting the new grants. Grant changes use the existing permission workflow and retain its audit.
