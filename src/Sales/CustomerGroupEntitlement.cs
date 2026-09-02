using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Sales
{
    public class CustomerGroupEntitlement : Entitlement
    {
        public const string UniqueID = "7726302211cb429983fa2496276e2f93";

        public const string RoleID = SalesRepresentativeRole.UniqueID;

        public const string NormalizedKey = "customergroup";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso ao grupo de clientes";

        public override string Key => NormalizedKey;
    }
}
