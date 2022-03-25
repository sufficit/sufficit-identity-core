using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Vendas
{
    public class SalesManagerRole : Role, IRole
    {
        public const string UniqueID = "dbbf95ce-36da-40d8-be43-4f8a97c6d896";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "Sales Manager";

        public override string NormalizedName => "salesmanager";

        string[] IRole.Filter => new string[] { NormalizedName, "gerente de vendas" };
    }
}
