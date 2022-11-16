using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public class ExpenseViewDirective : Directive
    {
        public const string UniqueID = "606a6636-8577-417a-a027-d6f4a8ed00ea";
        public const string RoleID = FinancialRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "visualizar despesas";

        public override string Key => "expenseview";
    }
}
