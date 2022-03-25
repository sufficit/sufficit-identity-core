using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public class TelephonyClientDirective : Directive
    {
        public const string UniqueID = "825e32b4-40d4-4f19-833c-7663bb9c26f7";
        public const string RoleID = TelephonyRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso ao cliente de telefonia";

        public override string Key => "telephonyclient";
    }
}
