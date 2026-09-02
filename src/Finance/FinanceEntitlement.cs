using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    /// <summary>
    /// Represents a entitlement that provides access to finance-related functionality.
    /// </summary>
    /// <remarks>This entitlement is identified by a unique ID and key, and is used to enable or manage finances
    /// operations.</remarks>
    public class FinanceEntitlement : Entitlement
    {
        public const string UniqueID = "20b48f3bd8394007bde438151740c6a9";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "acesso ao financeiro";

        public override string Key => "finance";
    }
}
