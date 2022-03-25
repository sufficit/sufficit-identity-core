using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class PhoneCallsDirective : Directive
    {
        public const string UniqueID = "cf3c66ab-db24-48b6-8c28-4603540286de";
        public const string RoleID = TelephonyRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "acesso a chamadas";

        public override string Key => "phonecalls";
    }
}
