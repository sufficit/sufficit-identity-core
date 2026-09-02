using Sufficit.Identity;
using System;

namespace Sufficit.Sales
{
    public class ClientAdminEntitlement : Entitlement
    {
        public const string UniqueID = "9d7c9980841a4c93bd648ade55a2f634";

        public const string RoleID = SalesManagerRole.UniqueID;

        public const string NormalizedKey = "clientadmin";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "controle de cliente";

        public override string Key => NormalizedKey;
    }
}
