using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class PhoneCallsEntitlement : Entitlement
    {
        public const string UniqueID = "cf3c66abdb2448b68c284603540286de";
        public const string RoleID = TelephonyRole.UniqueID;

        public const string NormalizedKey = "phonecalls";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "acesso a chamadas";

        public override string Key => NormalizedKey;
    }
}
