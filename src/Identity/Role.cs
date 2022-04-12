using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sufficit.Identity
{
    public static class Role
    {        
        public static bool Compare(this IRole source, string role)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                string newRole = role.ToLowerInvariant().Trim();
                if (source.Filter.Contains(newRole)){
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<IRole> Enumerator { get; }
            = Utils.GetCollectionOfType<IRole>();
    }
}
