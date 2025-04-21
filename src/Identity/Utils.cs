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
            var directive = Sufficit.Identity.Directive.Enumerator.FirstOrDefault(s => s.Key == key);
            if (directive == null) throw new ArgumentException($"directive key not found: {key}", nameof(key));

            return new UserPolicy(IDContext, directive);
        }

        /// <summary>
        ///     Processed roles from given policies
        /// </summary>
        /// <remarks>* useful for avoid insecure permissions</remarks>
        /// <exception cref="Exception">directives and roles ids not found</exception>
        public static HashSet<IRole> GetRoles (IEnumerable<UserPolicyBase> policies)
        {
            var roles = new HashSet<IRole>();
            var directives = new HashSet<IDirective>();
            foreach (var userpolicy in policies)
            {
                var directive = Sufficit.Identity.Directive.Enumerator.FirstOrDefault(s => s.ID == userpolicy.IDDirective);
                if (directive == null) throw new Exception($"directive id not found: {userpolicy.IDDirective}");

                if (directives.Add(directive))
                {
                    if (directive.IDRole != Guid.Empty)
                    {
                        var role = Sufficit.Identity.Role.Enumerator.FirstOrDefault(s => s.ID == directive.IDRole);
                        if (role == null) throw new Exception($"role id not found: {directive.IDRole}");
                        
                        roles.Add(role);
                    }
                }
            }
            return roles;
        }
    }
}
