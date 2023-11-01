using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public struct ManagerRole : IRole
    {
        public const string UniqueID = "9cdc6bbe-6be0-4a76-a15c-2638b4125175";

        public const string NormalizedName = "manager";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "System Manager";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new string[] { NormalizedName, "gerente" };
    }
}
