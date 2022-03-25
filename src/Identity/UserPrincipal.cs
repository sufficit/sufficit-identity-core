using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Sufficit.Identity;

namespace Sufficit.Identity
{
    public class UserPrincipal : System.Security.Claims.ClaimsPrincipal, IPrincipal
    {
        public UserPrincipal(IPrincipal principal) : base(principal)
        {
            Policies = new HashSet<UserPolicyBase>();
            Roles = new HashSet<IRole>();

            Populate();
        }

        public UserPrincipal(ClaimsIdentity identity)
        {
            this.AddIdentity(identity);

            Policies = new HashSet<UserPolicyBase>();
            Roles = new HashSet<IRole>();

            Populate();
        }

        public UserPrincipal(ClaimsPrincipal principal , AuthenticationUserOptions options)
        {
            var identity = new ClaimsIdentity(principal.Claims, options.AuthenticationType, options.NameClaim, string.Empty);
            this.AddIdentity(identity);

            Policies = new HashSet<UserPolicyBase>();
            Roles = new HashSet<IRole>();

            Populate();
        }

        public virtual ICollection<UserPolicyBase> Policies { get; }

        public virtual ICollection<IRole> Roles { get; }


        private void Populate()
        {
            var roles = new HashSet<Guid>();
            foreach (var claim in this.Claims)
            {
                if (claim.Type == Sufficit.Identity.ClaimTypes.Directive)
                {
                    var policy = claim.ToUserPolicy();
                    if (policy != null && !Policies.Contains(policy))
                    {
                        Policies.Add(policy);

                        if(policy.Directive.IDRole != Guid.Empty)
                            roles.Add(policy.Directive.IDRole);
                    }
                } else if(claim.Type == Sufficit.Identity.ClaimTypes.MicrosoftRole || claim.Type == Sufficit.Identity.ClaimTypes.Role)
                {
                    if(claim.Value == "administrator")
                    {
                        roles.Add(new AdministratorRole().ID);  
                    }
                }
            }

            foreach (var roleid in roles)
            {
                var role = Role.Enumerator.FirstOrDefault(s => s.ID == roleid);
                if (role != null)
                {
                    ((ClaimsIdentity)this.Identity).AddClaim(new Claim(Sufficit.Identity.ClaimTypes.Role, role.NormalizedName));
                    Roles.Add(role);
                }
            }
        }

        public override bool IsInRole(string role)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                string roleToCompare = role.ToLowerInvariant().Trim();
                return Roles.Any(s => s.Filter.Contains(roleToCompare));
            }
            return false;
        }
    }
}
