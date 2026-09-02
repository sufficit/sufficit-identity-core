using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public struct AdministratorRole : IRole
    {
        public const string UniqueID = "454ac90172d44eb19ff1547bc0339baf";
        public const string NormalizedName = "administrator";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "System Administrator";

        readonly string IRole.NormalizedName => NormalizedName;

        readonly string[] IRole.Filter => new string[] { NormalizedName, "administrador" };
    }
}
