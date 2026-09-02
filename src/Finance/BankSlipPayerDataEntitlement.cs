using System;

namespace Sufficit.Finance
{
    /// <summary>
    /// Allows audited access to protected payer snapshots.
    /// </summary>
    public class BankSlipPayerDataEntitlement : BankSlipManageEntitlement
    {
        public new const string UniqueID = "9c1cc91856d1420cb6c035be272e768b";
        public new const string RoleID = FinancialManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(RoleID);
        public override string Name => "acessar dados do pagador de boletos";
        public override string Key => "bankslippayerdata";
    }
}
