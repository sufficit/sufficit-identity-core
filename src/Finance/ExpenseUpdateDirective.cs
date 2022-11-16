using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public class ExpenseUpdateDirective : Directive
    {
        public const string UniqueID = "245a7668-6916-4946-92a0-edc702b27137";
        public const string RoleID = FinancialRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "atualizar despesas";

        public override string Key => "expenseupdate";
    }
}
