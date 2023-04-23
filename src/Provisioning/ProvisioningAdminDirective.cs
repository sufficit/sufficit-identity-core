using Sufficit.Identity;
using System;

namespace Sufficit.Provisioning
{
    public class ProvisioningAdminDirective : Directive
    {
        public const string UniqueID = "2353d733-50ff-4114-9626-d0120bcc5063";
        public const string RoleID = ProvisioningRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "administrar provisionamento";

        public override string Key => "provisioningadmin";
    }
}
