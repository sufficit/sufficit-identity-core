using Sufficit.Identity;
using Sufficit.Sales;
using System;

namespace Sufficit.Vendas
{
    public class ServiceUpdateDirective : Directive, IDirective
    {
        public const string UniqueID = "3aa87d8a-bd7a-4111-b279-c396471d7b37";
        public const string RoleID = SalesManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "ativar servico de cliente";

        public override string Key => "serviceupdate";
    }
}
