using Sufficit.Identity;
using System;

namespace Sufficit.Identity
{
    public class PolicyUpdateDirective : Directive
    {
        public const string UniqueID = "96910621-7b5c-40cd-af9a-0da4b78fe6f4";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "atualizar/limpar regras";

        public override string Key => "policyupdate";
    }
}
