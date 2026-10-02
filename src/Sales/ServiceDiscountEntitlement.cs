using Sufficit.Identity;
using System;
namespace Sufficit.Sales
{
    public class ServiceDiscountEntitlement : Entitlement, IEntitlement
    {
        public const string UniqueID = "a8688d5cb0de4dcfa058927f50aa4d7c";
        public const string RoleID = SalesManagerRole.UniqueID;
        public const string NormalizedKey = "servicediscount";
        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(RoleID);
        public override string Name => "aplicar desconto em servico";
        public override string Key => NormalizedKey;
    }
}
