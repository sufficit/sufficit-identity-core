using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Vendas
{
    public class SalesRepresentativeRole : Role, IRole
    {
        public const string UniqueID = "f56b4fa7-14ca-44ae-94b9-90296afaf48d";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "Sales Representative";

        public override string NormalizedName => "salesrepresentative";

        string[] IRole.Filter => new string[] { NormalizedName, "representante comercial" };
    }
}
