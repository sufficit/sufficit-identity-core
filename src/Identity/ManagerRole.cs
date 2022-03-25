using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public class ManagerRole : Role, IRole
    {
        public const string UniqueID = "9cdc6bbe-6be0-4a76-a15c-2638b4125175";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "System Manager";

        public override string NormalizedName => "manager";

        string[] IRole.Filter => new string[] { NormalizedName, "gerente" };
    }
}
