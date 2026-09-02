using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class AudioAdminEntitlement : Entitlement
    {
        public const string UniqueID = "d05f3e2b47bc4af58cb328517be91b6f";
        public const string RoleID = TelephonyRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "administrar áudios";

        public override string Key => "audioadmin";
    }
}
