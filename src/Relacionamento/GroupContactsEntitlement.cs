using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Relacionamento
{
    public class GroupContactsEntitlement : Entitlement
    {
        public const string UniqueID = "26b32b5b7ac143249b7183559bac1f1d";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "visualizar contatos por grupo";

        public override string Key => "groupcontacts";
    }
}
