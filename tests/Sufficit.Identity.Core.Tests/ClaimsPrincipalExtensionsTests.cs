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
        var principal = CreatePrincipal(userId, $"{AIUserDirective.NormalizedKey}:{Guid.Empty}");

        Assert.True(principal.HasPolicy<AIUserDirective>(userId));
        Assert.False(principal.HasPolicy<AIUserDirective>(Guid.NewGuid()));
        Assert.Equal(new[] { userId }, principal.HasDirective<AIUserDirective>());
        Assert.Contains(userId, principal.KnowingContexts());
    }

    [Fact]
    public void EmptyAIUserContextWithoutValidSubjectGrantsNoAccess()
    {
        var principal = CreatePrincipal(null, $"{AIUserDirective.NormalizedKey}:{Guid.Empty}");

        Assert.False(principal.HasPolicy(new AIUserDirective()));
        Assert.Empty(principal.HasDirective<AIUserDirective>());
        Assert.Empty(principal.KnowingContexts());
    }

    [Fact]
    public void ExplicitAIUserContextRemainsScopedToThatContext()
    {
        var userId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var principal = CreatePrincipal(userId, $"{AIUserDirective.NormalizedKey}:{customerId}");

        Assert.True(principal.HasPolicy<AIUserDirective>(customerId));
        Assert.False(principal.HasPolicy<AIUserDirective>(userId));
        Assert.Equal(new[] { customerId }, principal.HasDirective<AIUserDirective>());
    }

    [Fact]
    public void EmptyAIControlContextRemainsGlobal()
    {
        var principal = CreatePrincipal(Guid.NewGuid(), $"{AIControlDirective.NormalizedKey}:{Guid.Empty}");

        Assert.True(principal.HasPolicy<AIControlDirective>(Guid.NewGuid()));
        Assert.True(principal.HasPolicy(new AIControlDirective()));
        Assert.Equal(new[] { Guid.Empty }, principal.HasDirective<AIControlDirective>());
    }

    private static ClaimsPrincipal CreatePrincipal(Guid? userId, string directive)
    {
        var claims = new List<Claim>
        {
            new(Sufficit.Identity.ClaimTypes.Directive, directive)
        };

        if (userId.HasValue)
            claims.Add(new Claim(Sufficit.Identity.ClaimTypes.UserID, userId.Value.ToString()));

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "tests"));
    }
}
