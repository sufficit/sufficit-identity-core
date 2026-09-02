using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public class TelephonyAdminEntitlement : Entitlement
    {
        public const string UniqueID = "09394ab483384662a3d5dd3a75324032";        

        public const string RoleID = TelephonyAdminRole.UniqueID;

        public const string NormalizedKey = TelephonyAdminRole.NormalizedName;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso a administração de telefonia";

        public override string Key => NormalizedKey;
    }
}
