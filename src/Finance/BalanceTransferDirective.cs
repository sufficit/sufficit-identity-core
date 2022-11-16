using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public class BalanceTransferDirective : Directive
    {
        public const string UniqueID = "17f20ed1-1937-4f75-b419-31987a892ca0";
        public const string RoleID = FinancialRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "transferir saldo";

        public override string Key => "balancetransfer";
    }
}
