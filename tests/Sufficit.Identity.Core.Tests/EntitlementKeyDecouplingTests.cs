using Sufficit.Identity;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

/// <summary>
///     Guards the phase-0 decoupling of entitlement keys from role names
///     (decision 0006, PLAN-20260911-admin-suffix-migration.md).
/// </summary>
/// <remarks>
///     <para>
///         An entitlement key is persisted inside user claims
///         (<c>telephonyadmin:context-id</c>) while a role name lives in role
///         stores and <c>[Authorize(Roles = …)]</c> gates. Coupling them through
///         a shared constant meant that renaming the role would silently change
///         which persisted claims still resolve — an outage nobody asked for.
///     </para>
///     <para>
///         These tests pin the historical value with a literal so any future
///         rename must touch this file (and its migration plan) consciously.
///     </para>
/// </remarks>
public class EntitlementKeyDecouplingTests
{
    [Fact]
    public void TelephonyAdminKeyIsALiteralIndependentOfTheRoleName()
    {
        var entitlement = new Sufficit.Telephony.TelephonyAdminEntitlement();

        Assert.Equal("telephonyadmin", entitlement.Key);
        Assert.Equal("telephonyadmin", Sufficit.Telephony.TelephonyAdminEntitlement.NormalizedKey);
    }

    [Fact]
    public void EntitlementKeysNeverReferenceRoleConstantsAtRuntime()
    {
        // The keys may still *coincide* with a role name — that is expected
        // until the admin-suffix migration renames them. What must not happen
        // is a key CHANGING because a role constant changed. With the literal
        // in place, both values below are stable regardless of role renames.
        var entitlement = new Sufficit.Telephony.TelephonyAdminEntitlement();
        var role = new Sufficit.Telephony.TelephonyAdminRole();

        Assert.NotEqual(role.ID, entitlement.ID);
        Assert.Equal(Sufficit.Telephony.TelephonyAdminRole.UniqueID, Sufficit.Telephony.TelephonyAdminEntitlement.RoleID);
    }

    [Fact]
    public void OtherAdminEntitlementsKeepIndependentLiteralKeys()
    {
        Assert.Equal("audioadmin", new Sufficit.Telephony.AudioAdminEntitlement().Key);
        Assert.Equal("clientadmin", new Sufficit.Sales.ClientAdminEntitlement().Key);
        Assert.Equal("provisioningadmin", new Sufficit.Provisioning.ProvisioningAdminEntitlement().Key);
    }
}
