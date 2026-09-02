using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public struct TelephonySupervisorRole : IRole
    {
        public const string UniqueID = "df828011e0de4cb684812abf912115cf";
        
        public const string NormalizedName = "telephonysupervisor";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Telephony Supervisor";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "supervisor de telefonia" };
    }
}
