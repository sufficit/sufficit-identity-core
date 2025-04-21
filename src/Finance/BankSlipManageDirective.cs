using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    /// <summary>
    ///     BankSlip Management (emitt, cancel)
    /// </summary>
    public class BankSlipManageDirective : BankSlipDirective
    {
        public new const string UniqueID = "139e7b81-6b3c-450a-909c-cd2b7b25f543";
        public new const string RoleID = FinancialManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "gerenciar boletos";

        public override string Key => "bankslipmanage";
    }
}
