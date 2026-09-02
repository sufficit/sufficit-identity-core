using System;

namespace Sufficit.Finance
{
    /// <summary>
    /// Allows tenant-scoped administration of bank slip limits and providers.
    /// </summary>
    public class BankSlipSettingsEntitlement : BankSlipManageEntitlement
    {
        public new const string UniqueID = "0416f398e9df41909f704ca015ee5806";
        public new const string RoleID = FinancialManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(RoleID);
        public override string Name => "configurar boletos";
        public override string Key => "bankslipsettings";
    }
}
