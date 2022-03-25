using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    /// <summary>
    /// Realizar alterações em qualquer elemento do plano de discagem, entrada ou saída
    /// </summary>
    public class DialPlanUpdateDirective : Directive
    {
        public const string UniqueID = "2c248f08-08d8-4396-9d52-83525366486d";
        public const string RoleID = "e9099691-e1c7-498a-b3d6-364d01582176";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "atualizar plano de discagem";

        public override string Key => "dialplanupdate";
    }
}
