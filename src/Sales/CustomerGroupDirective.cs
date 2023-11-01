using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Sales
{
    public class CustomerGroupDirective : Directive
    {
        public const string UniqueID = "77263022-11cb-4299-83fa-2496276e2f93";
        public const string RoleID = SalesRepresentativeRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso ao grupo de clientes";

        public override string Key => "customergroup";
    }
}
