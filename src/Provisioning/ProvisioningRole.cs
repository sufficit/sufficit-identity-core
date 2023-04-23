using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Provisioning
{
    public struct ProvisioningRole : IRole
    {
        public const string UniqueID = "2c5db110-a92e-4757-8655-919d50aa043e";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "Provisioning";

        public string NormalizedName => "provisioning";

        string[] IRole.Filter => new string[] { NormalizedName };
    }
}
