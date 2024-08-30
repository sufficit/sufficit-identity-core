using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public struct TelephonyManagerRole : IRole
    {
        public const string UniqueID = "e9099691-e1c7-498a-b3d6-364d01582176";

        public const string NormalizedName = "telephonymanager";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Telephony Manager";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "gerente de telefonia" };
    }
}
