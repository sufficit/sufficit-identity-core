using System.Security.Claims;
using Sufficit.Identity;
using Sufficit.Telephony;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

/// <summary>
/// Both claim names must resolve to the same grant.
/// </summary>
/// <remarks>
/// The migration only works if a consumer accepts the standard container before
/// producers stop emitting the short name. Reading the claim is not enough:
/// the first attempt at this accepted <c>entitlements</c> in the enumeration
/// and then threw it away one call later, because the parser still rejected any
/// claim type other than <c>directive</c>. The reader looked correct and every
/// standard-named grant was silently discarded — which is the failure this file
/// exists to prevent.
/// </remarks>
public sealed class EntitlementClaimTests
{
    private const string Context = "11111111-1111-1111-1111-111111111111";

    private static ClaimsPrincipal PrincipalWith(string claimType) =>
        new(new ClaimsIdentity(
            [new Claim(claimType, $"{"phonecalls"}:{Context}")],
            "test"));

    [Fact]
    public void The_standard_container_is_honoured()
    {
        var principal = PrincipalWith(ClaimTypes.Entitlement);

        Assert.True(
            principal.HasEntitlement<PhoneCallsDirective>(Guid.Parse(Context)),
            "a grant carried in the RFC 9068 container was not honoured");
    }

    [Fact]
    public void The_historical_name_still_works()
    {
        var principal = PrincipalWith(ClaimTypes.Directive);

        Assert.True(principal.HasEntitlement<PhoneCallsDirective>(Guid.Parse(Context)));
    }

    [Fact]
    public void Both_names_resolve_to_the_same_grant()
    {
        // If these ever diverge, a consumer migrating from one name to the other
        // would silently change what it permits.
        Assert.Equal(
            PrincipalWith(ClaimTypes.Directive).HasEntitlement<PhoneCallsDirective>(),
            PrincipalWith(ClaimTypes.Entitlement).HasEntitlement<PhoneCallsDirective>());
    }

    [Fact]
    public void A_grant_in_another_context_is_not_honoured_here()
    {
        var principal = PrincipalWith(ClaimTypes.Entitlement);

        Assert.False(principal.HasEntitlement<PhoneCallsDirective>(
            Guid.Parse("00000000-0000-0000-0000-000000000000")));
    }

    [Fact]
    public void The_entitlement_alias_matches_the_original_check()
    {
        // The alias must stay an alias. A second implementation is how two call
        // sites in the same codebase start disagreeing.
        var principal = PrincipalWith(ClaimTypes.Entitlement);

        Assert.Equal(
            principal.HasDirective<PhoneCallsDirective>(),
            principal.HasEntitlement<PhoneCallsDirective>());
    }
}
