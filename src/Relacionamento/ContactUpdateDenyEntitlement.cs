using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Relacionamento
{
    public class ContactUpdateDenyEntitlement : Entitlement
    {
        public const string UniqueID = "9a989de21f97449caf296e12d4d91491";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "não alterar contato";

        public override string Key => "contactdeny";
    }
}
