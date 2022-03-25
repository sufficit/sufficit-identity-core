using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Relacionamento
{
    public class ContactUpdateDenyDirective : Directive
    {
        public const string UniqueID = "9a989de2-1f97-449c-af29-6e12d4d91491";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "não alterar contato";

        public override string Key => "contactdeny";
    }
}
