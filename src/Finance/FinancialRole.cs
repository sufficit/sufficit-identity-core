using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public struct FinancialRole : IRole
    {
        public const string UniqueID = "547c85dc-15fc-4730-9377-98e1bacd0899";
        public const string NormalizedName = "financial";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Financial";

        readonly string IRole.NormalizedName => NormalizedName;

        readonly string[] IRole.Filter => new string[] { NormalizedName, "financeiro" };
    }
}
