using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public class BalanceViewDirective : Directive
    {
        public const string UniqueID = "1c8a1f49-6f3e-4979-8885-d52fc79f0dee";
        public const string RoleID = FinancialRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "visualizar saldo";

        public override string Key => "balanceview";
    }
}
