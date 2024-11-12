using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Sales
{
    public struct SalesManagerRole : IRole
    {
        public const string UniqueID = "dbbf95ce-36da-40d8-be43-4f8a97c6d896";
        public const string NormalizedName = "salesmanager";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Sales Manager";

        readonly string IRole.NormalizedName => NormalizedName;

        readonly string[] IRole.Filter => new string[] { NormalizedName, "gerente de vendas" };
    }
}
