using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Financeiro
{
    public class FinancialManagerRole : Role, IRole
    {
        public const string UniqueID = "1ef82b4a-532e-4225-b3db-7619c5cd8870";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "Financial Manager";

        public override string NormalizedName => "financialmanager";

        string[] IRole.Filter => new string[] { NormalizedName, "gerente financeiro" };
    }
}
