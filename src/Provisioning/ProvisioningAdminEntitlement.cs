using Sufficit.Identity;
using System;

namespace Sufficit.Provisioning
{
    public class ProvisioningAdminEntitlement : Entitlement
    {
        public const string UniqueID = "2353d73350ff41149626d0120bcc5063";
        public const string RoleID = ProvisioningRole.UniqueID;

        public override Guid ID { get; } = Guid.Parse(UniqueID);

        public override Guid IDRole { get; } = Guid.Parse(RoleID); 

        public override string Name { get; } = "administrar provisionamento";

        public override string Key { get; } = "provisioningadmin";
    }
}
