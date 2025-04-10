using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

namespace Sufficit.Identity
{
    public class UserPrincipal : System.Security.Claims.ClaimsPrincipal, IPrincipal
    {
        public UserPrincipal(IPrincipal principal) : base(principal)
        {
            Policies = new HashSet<UserPolicyBase>();
            Roles = new HashSet<IRole>();

            Populate(this);
        }
        
        public UserPrincipal(IIdentity identity) : base(identity)
        {
            Policies = new HashSet<UserPolicyBase>();
            Roles = new HashSet<IRole>();

            Populate(this);
        }
        
        public UserPrincipal (ClaimsPrincipal principal, AuthenticationUserOptions options)
        {
            Policies = new HashSet<UserPolicyBase>();
            Roles = new HashSet<IRole>();

            var identity = new ClaimsIdentity(principal.Claims, options.AuthenticationType, options.NameClaim, string.Empty);
            AddIdentity(identity);
        }

        public virtual ICollection<UserPolicyBase> Policies { get; }

        public virtual ICollection<IRole> Roles { get; }

        static void Populate(UserPrincipal user)
        {
            var roles = new HashSet<Guid>();
            foreach (var claim in user.Claims)
            {
                if (claim.Type == ClaimTypes.Directive)
                {
                    foreach (var text in DeserializeSingleOrList(claim.Value))
                    {
                        var policy = new Claim(ClaimTypes.Directive, text).ToUserPolicy();
                        if (policy != null && !user.Policies.Contains(policy))
                        {
                            user.Policies.Add(policy);

                            if (policy.Directive.IDRole != Guid.Empty)
                                roles.Add(policy.Directive.IDRole);
                        }
                    }
                } 
                else if (claim.Type == Sufficit.Identity.ClaimTypes.MicrosoftRole || claim.Type == Sufficit.Identity.ClaimTypes.Role)
                {
                    foreach (var text in DeserializeSingleOrList(claim.Value))
                    {
                        if (text == Sufficit.Identity.AdministratorRole.NormalizedName)
                            roles.Add(Guid.Parse(AdministratorRole.UniqueID));
                        else if (text == Sufficit.Identity.ManagerRole.NormalizedName)
                            roles.Add(Guid.Parse(ManagerRole.UniqueID));
                    }                    
                }
            }

            if (roles.Count > 0)
            {
                // For compatibility to another systems
                if (user.Identity is ClaimsIdentity identity)
                {
                    // avoid enumerate multiple times
                    var claims = identity.Claims.ToList();

                    foreach (var roleid in roles)
                    {
                        var role = Role.Enumerator.FirstOrDefault(s => s.ID == roleid);
                        if (role != null)
                        {
                            user.Roles.Add(role);

                            if (!claims.Any(s => s.Type == ClaimTypes.Role && s.Value == role.NormalizedName))
                            {
                                var newClaim = new Claim(Sufficit.Identity.ClaimTypes.Role, role.NormalizedName);
                                identity.AddClaim(newClaim);
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<string> DeserializeSingleOrList(string jsonText)
        {
            if (!jsonText.StartsWith("["))
                yield return jsonText;
            else
            {
                foreach (var text in JsonSerializer.Deserialize<string[]>(jsonText) ?? Array.Empty<string>())
                    if(!string.IsNullOrWhiteSpace(text))
                        yield return text;
            }
        }

        public readonly static char[] RoleSeparators = { ',', '|' };

        /// <summary>
        ///     Check if user is in role, or roles using the <see cref="RoleSeparators"/>
        /// </summary>
        public override bool IsInRole(string role)
        {
            string? roleToCompare = role?.ToLowerInvariant().Trim();
            if (!string.IsNullOrWhiteSpace(roleToCompare))
            {
                // test for role in the list of roles
                if (roleToCompare.IndexOfAny(RoleSeparators) != -1)
                {
                    var roles = roleToCompare.Split(RoleSeparators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var r in roles)
                    {
                        if (Roles.Any(s => s.Filter.Contains(r.Trim())))
                            return true;
                    }
                }

                // test for a single role
                else
                {
                    return Roles.Any(s => s.Filter.Contains(roleToCompare));
                }
            }
            
            return false;
        }

        public override void AddIdentity(ClaimsIdentity identity)
        {
            base.AddIdentity(identity);
            Populate(this);
        }
    }
}
