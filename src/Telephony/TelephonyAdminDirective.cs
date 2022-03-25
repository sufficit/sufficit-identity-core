using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public class TelephonyAdminDirective : Directive
    {
        public const string UniqueID = "09394ab4-8338-4662-a3d5-dd3a75324032";
        public const string RoleID = TelephonyAdminRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso a administração de telefonia";

        public override string Key => "telephonyadmin";
    }
}
