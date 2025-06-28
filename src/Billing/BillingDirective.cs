using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Billing
{
    /// <summary>
    /// Represents a directive that provides access to billing-related functionality.
    /// </summary>
    /// <remarks>This directive is identified by a unique ID and key, and is used to enable or manage billing
    /// operations.</remarks>
    public class BillingDirective : Directive
    {
        public const string UniqueID = "20b48f3b-d839-4007-bde4-38151740c6a9";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "acesso ao faturamento";

        public override string Key => "billing";
    }
}
