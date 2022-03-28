using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Vendas
{
    public struct SalesManagerRole : IRole
    {
        public const string UniqueID = "dbbf95ce-36da-40d8-be43-4f8a97c6d896";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "Sales Manager";

        public string NormalizedName => "salesmanager";

        string[] IRole.Filter => new string[] { NormalizedName, "gerente de vendas" };
    }
}
