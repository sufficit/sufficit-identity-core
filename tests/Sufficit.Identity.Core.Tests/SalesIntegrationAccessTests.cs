using System.Security.Claims;
using Sufficit.Identity;
using Sufficit.Sales;
using Xunit;

public sealed class SalesIntegrationAccessTests
{
    private static ClaimsPrincipal User(string key, Guid context, bool authenticated = true) => new(new ClaimsIdentity(
        new[] { new Claim(Sufficit.Identity.ClaimTypes.Entitlement, key + ":" + context) }, authenticated ? "test" : null));

    [Fact]
    public void ReadAndRetryAreIndependentAndDoNotDeriveAnyPlatformRole()
    {
        var context = Guid.NewGuid(); var other = Guid.NewGuid();
        var read = User(SalesIntegrationAuditEntitlement.NormalizedKey, context);
        var retry = User(SalesIntegrationRetryEntitlement.NormalizedKey, context);
        Assert.True(SalesIntegrationAccess.CanRead(read, context));
        Assert.False(SalesIntegrationAccess.CanRetry(read, context));
        Assert.True(SalesIntegrationAccess.CanRetry(retry, context));
        Assert.False(SalesIntegrationAccess.CanRead(retry, context));
        Assert.False(SalesIntegrationAccess.CanRead(read, other));
        Assert.False(SalesIntegrationAccess.CanRetry(retry, other));
        Assert.Empty(Utils.GetRoles(read.GetUserPolicies()));
        Assert.Empty(Utils.GetRoles(retry.GetUserPolicies()));
        Assert.False(read.HasPolicy<Sufficit.Finance.BalanceViewEntitlement>(context));
        Assert.False(read.HasPolicy<Sufficit.Telephony.TelephonyClientEntitlement>(context));
    }

    [Fact]
    public void ExplicitGlobalGrantStillRequiresAuthenticationAndANonemptyQueryContext()
    {
        var global = User(SalesIntegrationAuditEntitlement.NormalizedKey, Guid.Empty);
        Assert.True(SalesIntegrationAccess.CanRead(global, Guid.NewGuid()));
        Assert.False(SalesIntegrationAccess.CanRead(global, Guid.Empty));
        Assert.False(SalesIntegrationAccess.CanRead(User(SalesIntegrationAuditEntitlement.NormalizedKey, Guid.Empty, false), Guid.NewGuid()));
        var malformed = new ClaimsPrincipal(new ClaimsIdentity(new[] {
            new Claim(Sufficit.Identity.ClaimTypes.Entitlement, SalesIntegrationAuditEntitlement.NormalizedKey + ":invalid") }, "test"));
        Assert.False(SalesIntegrationAccess.CanRead(malformed, Guid.NewGuid()));
    }
}
