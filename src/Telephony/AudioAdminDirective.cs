using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class AudioAdminDirective : Directive
    {
        public const string UniqueID = "d05f3e2b-47bc-4af5-8cb3-28517be91b6f";
        public const string RoleID = TelephonyRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "administrar áudios";

        public override string Key => "audioadmin";
    }
}
