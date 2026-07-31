using System;

namespace Sufficit.Finance
{
    /// <summary>
    /// Allows audited access to protected payer snapshots.
    /// </summary>
    public class BankSlipPayerDataDirective : BankSlipManageDirective
    {
        public new const string UniqueID = "9c1cc918-56d1-420c-b6c0-35be272e768b";
        public new const string RoleID = FinancialManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(RoleID);
        public override string Name => "acessar dados do pagador de boletos";
        public override string Key => "bankslippayerdata";
    }
}
