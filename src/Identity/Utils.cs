using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Sufficit.Identity
{
    public static partial class Utils
    {
        /// <summary>
        ///     Generate a new <see cref="UserPolicy" /> from key and guid context as (name):(context)
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static UserPolicy ToUserPolicy(string key, string context)
        {
            if (!Guid.TryParse(context, out Guid IDContext)) throw new ArgumentException($"invalid context guid format: {context}", nameof(context));
            var entitlement = Sufficit.Identity.Entitlement.Enumerator.FirstOrDefault(s => s.Key == key);
            if (entitlement == null) throw new ArgumentException($"entitlement key not found: {key}", nameof(key));

            return new UserPolicy(IDContext, entitlement);
        }

        /// <summary>
        ///     Processed roles from given policies
        /// </summary>
        /// <remarks>* useful for avoid insecure permissions</remarks>
        /// <exception cref="Exception">entitlements and roles ids not found</exception>
        public static HashSet<IRole> GetRoles (IEnumerable<UserPolicyBase> policies)
        {
            var roles = new HashSet<IRole>();
            var entitlements = new HashSet<IEntitlement>();
            foreach (var userpolicy in policies)
            {
                var entitlement = Sufficit.Identity.Entitlement.Enumerator.FirstOrDefault(s => s.ID == userpolicy.IDDirective);
                if (entitlement == null) throw new Exception($"entitlement id not found: {userpolicy.IDDirective}");

                if (entitlements.Add(entitlement))
                {
                    if (entitlement.IDRole != Guid.Empty)
                    {
                        var role = Sufficit.Identity.Role.Enumerator.FirstOrDefault(s => s.ID == entitlement.IDRole);
                        if (role == null) throw new Exception($"role id not found: {entitlement.IDRole}");
                        
                        roles.Add(role);
                    }
                }
            }
            return roles;
        }
    }
}
