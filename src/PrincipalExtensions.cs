using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sufficit.Identity
{
    public static class PrincipalExtensions
    {
        public static bool IsManager(this ClaimsPrincipal principal)
        {
            return
                principal.IsInRole(ManagerRole.NormalizedName) ||
                principal.IsInRole(AdministratorRole.NormalizedName);
        }
    }
}
