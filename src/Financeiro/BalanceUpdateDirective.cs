using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Financeiro
{
    public class BalanceUpdateDirective : Directive
    {
        public const string UniqueID = "cf50a644-9347-48d9-9296-18973a252a55";
        public const string RoleID = FinancialRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "realizar movimentação financeira";

        public override string Key => "balanceupdate";
    }
}
