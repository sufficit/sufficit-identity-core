using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public struct TelephonyRole : IRole
    {
        public const string UniqueID = "63e90377-5a05-463c-a674-9071dd90817c";

        public const string NormalizedName = "telephony";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Telephony";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "telefonia" };
    }
}
