using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    /// <summary>
    ///     BankSlip Access (view basic info, pdf link)
    /// </summary>
    /// <remarks>*wrong key name in code, change in future</remarks>
    public class BankSlipEntitlement : Entitlement
    {
        public const string UniqueID = "1cea282f5b3645d685e6d1ad866d2b27";
        public const string RoleID = FinancialRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso a boletos";

        public override string Key => "bankbillet";
    }
}
