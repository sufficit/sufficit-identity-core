using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public static class UserPrincipalExtensions
    {
        public static bool IsInRole(this UserPrincipal source, IRole role) => source.Roles.Contains(role);

        public static bool IsInRole<T>(this UserPrincipal source) where T : IRole, new()
        {
            return source.Roles.Contains(new T());
        }

        public static bool IsInRole(this UserPrincipal source, IEnumerable<IRole> roles)
        {
            foreach (var role in roles)
                if (source.IsInRole(role)) return true;
            return false;
        }
    }
}
