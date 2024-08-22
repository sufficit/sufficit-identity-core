using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Sales
{
    public struct SalesRepresentativeRole : IRole
    {
        public const string UniqueID = "f56b4fa7-14ca-44ae-94b9-90296afaf48d";

        public const string NormalizedName = "salesrepresentative";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "Sales Representative";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new string[] { NormalizedName, "representante comercial" };
    }
}
