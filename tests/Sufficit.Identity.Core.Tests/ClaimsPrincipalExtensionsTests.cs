using System.Security.Claims;
using Sufficit.AI;
using Sufficit.Identity;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void EmptyAIUserContextResolvesToAuthenticatedUser()
    {
        var userId = Guid.NewGuid();
        var principal = CreatePrincipal(userId, $"{AIUserEntitlement.NormalizedKey}:{Guid.Empty}");

        Assert.True(principal.HasPolicy<AIUserEntitlement>(userId));
        Assert.False(principal.HasPolicy<AIUserEntitlement>(Guid.NewGuid()));
        Assert.Equal(new[] { userId }, principal.HasEntitlement<AIUserEntitlement>());
        Assert.Contains(userId, principal.KnowingContexts());
    }

    [Fact]
    public void EmptyAIUserContextWithoutValidSubjectGrantsNoAccess()
    {
        var principal = CreatePrincipal(null, $"{AIUserEntitlement.NormalizedKey}:{Guid.Empty}");

        Assert.False(principal.HasPolicy(new AIUserEntitlement()));
        Assert.Empty(principal.HasEntitlement<AIUserEntitlement>());
        Assert.Empty(principal.KnowingContexts());
    }

    [Fact]
    public void ExplicitAIUserContextRemainsScopedToThatContext()
    {
        var userId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var principal = CreatePrincipal(userId, $"{AIUserEntitlement.NormalizedKey}:{customerId}");

        Assert.True(principal.HasPolicy<AIUserEntitlement>(customerId));
        Assert.False(principal.HasPolicy<AIUserEntitlement>(userId));
        Assert.Equal(new[] { customerId }, principal.HasEntitlement<AIUserEntitlement>());
    }

    [Fact]
    public void EmptyAIControlContextRemainsGlobal()
    {
        var principal = CreatePrincipal(Guid.NewGuid(), $"{AIControlEntitlement.NormalizedKey}:{Guid.Empty}");

        Assert.True(principal.HasPolicy<AIControlEntitlement>(Guid.NewGuid()));
        Assert.True(principal.HasPolicy(new AIControlEntitlement()));
        Assert.Equal(new[] { Guid.Empty }, principal.HasEntitlement<AIControlEntitlement>());
    }

    private static ClaimsPrincipal CreatePrincipal(Guid? userId, string entitlement)
    {
        var claims = new List<Claim>
        {
            new(Sufficit.Identity.ClaimTypes.Directive, entitlement)
        };

        if (userId.HasValue)
            claims.Add(new Claim(Sufficit.Identity.ClaimTypes.UserID, userId.Value.ToString()));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "tests"));
    }
}
