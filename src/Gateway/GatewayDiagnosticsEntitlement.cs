using Sufficit.Identity;
using System;

namespace Sufficit.Gateway
{
    /// <summary>
    /// Allows an administrator to execute audited, allow-listed provider diagnostics.
    /// </summary>
    public sealed class GatewayDiagnosticsEntitlement : Entitlement
    {
        public const string UniqueID = "1dc24b3ac72440c19793cc8a74ad61c9";
        public const string RoleID = AdministratorRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(RoleID);
        public override string Name => "depurar gateways";
        public override string Key => "gatewaydiagnostics";
    }
}
