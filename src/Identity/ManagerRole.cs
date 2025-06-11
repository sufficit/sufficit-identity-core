using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    /// <summary>
    /// Represents the role of a system manager within the application.
    /// </summary>
    /// <remarks>This struct provides information about the system manager role, including its unique
    /// identifier, display name, normalized name, and associated filters. It implements the <see cref="IRole"/>
    /// interface to ensure compatibility with role-based systems.</remarks>
    public struct ManagerRole : IRole
    {
        public const string UniqueID = "9cdc6bbe-6be0-4a76-a15c-2638b4125175";
        public const string NormalizedName = "manager";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "System Manager";

        readonly string IRole.NormalizedName => NormalizedName;

        readonly string[] IRole.Filter => new string[] { NormalizedName, "gerente" };
    }
}
