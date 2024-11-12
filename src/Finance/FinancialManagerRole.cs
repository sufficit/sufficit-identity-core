using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public struct FinancialManagerRole : IRole
    {
        public const string UniqueID = "1ef82b4a-532e-4225-b3db-7619c5cd8870";
        public const string NormalizedName = "financialmanager";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Financial Manager";

        readonly string IRole.NormalizedName => NormalizedName;

        readonly string[] IRole.Filter => new string[] { NormalizedName, "gerente financeiro" };
    }
}
