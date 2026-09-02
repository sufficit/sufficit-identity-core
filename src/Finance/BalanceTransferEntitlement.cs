using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public class BalanceTransferEntitlement : Entitlement
    {
        public const string UniqueID = "17f20ed119374f75b41931987a892ca0";
        public const string RoleID = FinancialRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "transferir saldo";

        public override string Key => "balancetransfer";
    }
}
