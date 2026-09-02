using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Provisioning
{
    public struct ProvisioningRole : IRole
    {
        public const string UniqueID = "2c5db110a92e47578655919d50aa043e";

        public const string NormalizedName = "provisioning";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Provisioning";

        readonly string IRole.NormalizedName => NormalizedName;

        readonly string[] IRole.Filter => new string[] { NormalizedName };
    }
}
