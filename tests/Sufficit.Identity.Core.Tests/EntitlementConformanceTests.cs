using Sufficit.Identity;
using Sufficit.Telephony;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

/// <summary>
/// The reference run of the shared conformance cases.
/// </summary>
/// <remarks>
/// Every application runs this same set inside its own suite. This one proves
/// the expectations themselves are right; the copies in each application prove
/// that application agrees.
/// </remarks>
public sealed class EntitlementConformanceTests
{
    public static TheoryData<EntitlementConformance.Case> Cases()
    {
        var data = new TheoryData<EntitlementConformance.Case>();
        foreach (var item in EntitlementConformance.Cases) data.Add(item);
        return data;
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void Every_application_must_reach_this_decision(EntitlementConformance.Case item)
    {
        var actual = item.Principal()
            .HasEntitlement<PhoneCallsEntitlement>(Guid.Parse(item.Context));

        Assert.Equal(item.Expected, actual);
    }

    [Fact]
    public void The_case_set_covers_both_answers()
    {
        // A set that only ever expects "denied" would pass in an application that
        // denies everything.
        Assert.Contains(EntitlementConformance.Cases, c => c.Expected);
        Assert.Contains(EntitlementConformance.Cases, c => !c.Expected);
    }
}
