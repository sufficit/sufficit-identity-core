using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    /// <summary>
    ///     Portability Management
    /// </summary>
    public class PortabilityDirective : Directive
    {
        public const string UniqueID = "fe921659-0606-4c43-982f-0c3baa5cf90a";
        public const string RoleID = TelephonyManagerRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "gerenciar processos de portabilidade";

        public override string Key => "portability";
    }
}
