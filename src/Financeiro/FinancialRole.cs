using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Financeiro
{
    public class FinancialRole : Role, IRole
    {
        public const string UniqueID = "547c85dc-15fc-4730-9377-98e1bacd0899";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "Financial";

        public override string NormalizedName => "financial";

        string[] IRole.Filter => new string[] { NormalizedName, "financeiro" };
    }
}
