using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public struct TelephonyManagerRole : IRole
    {
        public const string UniqueID = "e9099691-e1c7-498a-b3d6-364d01582176";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "Telephony Manager";

        public string NormalizedName => "telephonymanager";

        string[] IRole.Filter => new[] { NormalizedName, "gerente de telefonia" };
    }
}
