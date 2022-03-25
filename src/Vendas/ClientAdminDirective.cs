using Sufficit.Identity;
using System;

namespace Sufficit.Vendas
{
    public class ClientAdminDirective : Directive
    {
        public const string UniqueID = "9d7c9980-841a-4c93-bd64-8ade55a2f634";
        public const string RoleID = SalesManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "controle de cliente";

        public override string Key => "clientadmin";
    }
}
