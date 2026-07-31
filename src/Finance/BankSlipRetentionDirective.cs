using System;

namespace Sufficit.Finance
{
    /// <summary>
    /// Allows tenant-scoped administration of bank slip retention and legal holds.
    /// </summary>
    public class BankSlipRetentionDirective : BankSlipManageDirective
    {
        public new const string UniqueID = "7fb582fa-0892-42c1-baa2-0d53e20b29df";
        public new const string RoleID = FinancialManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(RoleID);
        public override string Name => "gerenciar retenção de boletos";
        public override string Key => "bankslipretention";
    }
}
