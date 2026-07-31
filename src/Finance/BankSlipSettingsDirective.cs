using System;

namespace Sufficit.Finance
{
    /// <summary>
    /// Allows tenant-scoped administration of bank slip limits and providers.
    /// </summary>
    public class BankSlipSettingsDirective : BankSlipManageDirective
    {
        public new const string UniqueID = "0416f398-e9df-4190-9f70-4ca015ee5806";
        public new const string RoleID = FinancialManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(RoleID);
        public override string Name => "configurar boletos";
        public override string Key => "bankslipsettings";
    }
}
