using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Relacionamento
{
    public class GroupContactsDirective : Directive
    {
        public const string UniqueID = "26b32b5b-7ac1-4324-9b71-83559bac1f1d";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "visualizar contatos por grupo";

        public override string Key => "groupcontacts";
    }
}
