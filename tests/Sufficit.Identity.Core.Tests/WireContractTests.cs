using System.Reflection;
using Sufficit.Identity;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

/// <summary>
/// Renaming a type is a refactor. Renaming a wire value is a contract change.
/// </summary>
/// <remarks>
/// The rename from <c>Directive</c> to <c>Entitlement</c> made that distinction
/// concrete: a mechanical replacement also rewrote the string literal behind
/// <c>ClaimTypes.Directive</c>, turning the claim name on the wire from
/// <c>directive</c> into <c>entitlement</c>. It compiled. Every consumer still
/// reading the old name would simply have stopped seeing grants.
/// <para>
/// These assertions pin the values that travel between services, so the next
/// mechanical pass cannot quietly move one.
/// </para>
/// </remarks>
public sealed class WireContractTests
{
    [Fact]
    public void The_legacy_claim_name_is_still_directive()
    {
        // Consumers migrate to the standard container on their own schedule.
        // Producers stop emitting this only after the last one has moved.
        Assert.Equal("directive", ClaimTypes.Directive);
    }

    [Fact]
    public void The_standard_claim_name_is_the_rfc_container()
    {
        // RFC 9068 section 2.2.3.2, plural, with SCIM semantics from RFC 7643.
        Assert.Equal("entitlements", ClaimTypes.Entitlement);
    }

    [Fact]
    public void Entitlement_keys_and_identifiers_are_stable()
    {
        // Key is the wire identifier and ID is the equality key. Both are
        // declared explicitly precisely so a class rename cannot touch them.
        var telephony = new Sufficit.Telephony.PhoneCallsEntitlement();

        Assert.Equal("phonecalls", telephony.Key);
        Assert.Equal(
            Guid.Parse("cf3c66ab-db24-48b6-8c28-4603540286de"),
            telephony.ID);
    }

    [Fact]
    public void No_two_entitlements_share_an_identifier()
    {
        // Reuse of a retired identifier silently grants the old permission to
        // the new thing — the one mistake in this file with no recovery path.
        var byIdentifier = new Dictionary<Guid, string>();
        var collisions = new List<string>();

        foreach (var entitlement in Entitlement.Enumerator)
        {
            if (byIdentifier.TryGetValue(entitlement.ID, out var existing))
            {
                collisions.Add(
                    $"{entitlement.GetType().Name} reuses the identifier of {existing}");
                continue;
            }

            byIdentifier[entitlement.ID] = entitlement.GetType().Name;
        }

        Assert.True(collisions.Count == 0, string.Join("\n", collisions));
    }

    [Fact]
    public void No_two_entitlements_share_a_key()
    {
        var byKey = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var collisions = new List<string>();

        foreach (var entitlement in Entitlement.Enumerator)
        {
            if (byKey.TryGetValue(entitlement.Key, out var existing))
            {
                collisions.Add(
                    $"{entitlement.GetType().Name} reuses the key '{entitlement.Key}' "
                    + $"of {existing}");
                continue;
            }

            byKey[entitlement.Key] = entitlement.GetType().Name;
        }

        Assert.True(collisions.Count == 0, string.Join("\n", collisions));
    }

    [Fact]
    public void Discovery_finds_every_entitlement_exactly_once()
    {
        // Discovery is reflection over IEntitlement. A compatibility subclass
        // kept beside a renamed type would show up here as a duplicate — which
        // is why the rename was done cleanly instead of with aliases.
        var discovered = Entitlement.Enumerator.ToList();
        var declared = typeof(Entitlement).Assembly
            .GetTypes()
            .Where(type => typeof(IEntitlement).IsAssignableFrom(type)
                && !type.IsAbstract
                && !type.IsInterface
                && type.GetConstructor(Type.EmptyTypes) is not null)
            .ToList();

        Assert.Equal(
            declared.Count(type => !typeof(UserPolicyBase).IsAssignableFrom(type)
                && type != typeof(EntitlementBase)),
            discovered.Count);
    }
}
