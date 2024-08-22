using Sufficit.Identity;
using System;

namespace Sufficit.Sales
{
    public class ServiceUpdateDirective : Directive, IDirective
    {
        public const string UniqueID = "3aa87d8a-bd7a-4111-b279-c396471d7b37";

        public const string RoleID = SalesManagerRole.UniqueID;

        public const string NormalizedKey = "serviceupdate";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "ativar servico de cliente";

        public override string Key => NormalizedKey;
    }
}
