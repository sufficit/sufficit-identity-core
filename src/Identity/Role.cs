using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sufficit.Identity
{
    public static class Role
    {        
        public static IEnumerable<IRole> Enumerator { get; }
            = Sufficit.Utils.GetCollectionOfType<IRole>();

        public static bool Compare(this IRole source, string? role)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                string newRole = role!.ToLowerInvariant().Trim();
                if (Guid.TryParse(role, out Guid roleId))
                {
                    if (source.ID == roleId)
                        return true;
                }
                else if (source.Filter.Contains(newRole)){
                    return true;
                }
            }
            return false;
        }
    }
}
