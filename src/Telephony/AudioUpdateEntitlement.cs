using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class AudioUpdateEntitlement : Entitlement
    {
        public const string UniqueID = "b327fb3866f240639cd0da1e4241d2d5";
        public const string RoleID = TelephonyRole.UniqueID;

        public const string NormalizedKey = "audioupdate";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "atualizar áudio";

        public override string Key => NormalizedKey;
    }
}
