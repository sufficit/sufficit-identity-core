using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Financeiro
{
    public struct FinancialManagerRole : IRole
    {
        public const string UniqueID = "1ef82b4a-532e-4225-b3db-7619c5cd8870";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "Financial Manager";

        public string NormalizedName => "financialmanager";

        string[] IRole.Filter => new string[] { NormalizedName, "gerente financeiro" };
    }
}
