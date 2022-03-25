using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sufficit.Identity
{
    public abstract class Role : IRole
    {
        public abstract Guid ID { get; }

        public virtual string Name => NormalizedName;

        public virtual string NormalizedName { get; }

        string[] IRole.Filter => new[] { NormalizedName };
        
        public bool Compare(string role)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                string newRole = role.ToLowerInvariant().Trim();
                if (((IRole)this).Filter.Contains(newRole)){
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<IRole> Enumerator { get; } = Utils.GetEnumerableOfType<IRole>();
    }
}
