using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public struct TelephonyAdminRole : IRole
    {
        public const string UniqueID = "0a23a38a-d4e4-4b64-98fa-9cb6a4aaa61f";

        public const string NormalizedName = "telephonyadmin";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "Telephony Administrator";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "administrador de telefonia" };
    }
}
