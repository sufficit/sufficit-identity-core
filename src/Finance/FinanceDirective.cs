using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    /// <summary>
    /// Represents a directive that provides access to finance-related functionality.
    /// </summary>
    /// <remarks>This directive is identified by a unique ID and key, and is used to enable or manage finances
    /// operations.</remarks>
    public class FinanceDirective : Directive
    {
        public const string UniqueID = "20b48f3b-d839-4007-bde4-38151740c6a9";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "acesso ao financeiro";

        public override string Key => "finance";
    }
}
