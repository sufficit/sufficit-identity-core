﻿using Sufficit.Identity;
using System;

namespace Sufficit.Provisioning
{
    public class ProvisioningAdminDirective : Directive
    {
        public const string UniqueID = "2353d733-50ff-4114-9626-d0120bcc5063";
        public const string RoleID = ProvisioningRole.UniqueID;

        public override Guid ID { get; } = Guid.Parse(UniqueID);

        public override Guid IDRole { get; } = Guid.Parse(RoleID); 

        public override string Name { get; } = "administrar provisionamento";

        public override string Key { get; } = "provisioningadmin";
    }
}
