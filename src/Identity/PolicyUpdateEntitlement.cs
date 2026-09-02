using Sufficit.Identity;
using System;

namespace Sufficit.Identity
{
    public class PolicyUpdateEntitlement : Entitlement
    {
        public const string UniqueID = "969106217b5c40cdaf9a0da4b78fe6f4";

        public const string NormalizedKey = "policyupdate";
        
        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "atualizar/limpar regras";

        public override string Key => NormalizedKey;
    }
}
