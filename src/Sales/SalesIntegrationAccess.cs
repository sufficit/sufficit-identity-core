using Sufficit.Identity;
using System;
using System.Security.Claims;

namespace Sufficit.Sales
{

/// <summary>Scoped transport permissions for ordinary authenticated principals; not a financial or telephony delegation.</summary>
public static class SalesIntegrationAccess
{
    /// <summary>Checks only the audit grant or an existing administrator role.</summary>
    public static bool CanRead(ClaimsPrincipal user, Guid contextId) => contextId != Guid.Empty &&
        user.Identity?.IsAuthenticated == true && (user.IsInRole(AdministratorRole.NormalizedName) ||
        user.HasPolicy<SalesIntegrationAuditEntitlement>(contextId));
    /// <summary>Checks only the retry grant or an existing administrator role.</summary>
    public static bool CanRetry(ClaimsPrincipal user, Guid contextId) => contextId != Guid.Empty &&
        user.Identity?.IsAuthenticated == true && (user.IsInRole(AdministratorRole.NormalizedName) ||
        user.HasPolicy<SalesIntegrationRetryEntitlement>(contextId));
}

}
