using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sufficit.Identity
{
    public static class PrincipalExtensions
    {
        public static bool IsManager (this ClaimsPrincipal? principal)
        {
            if (principal == null) return false;

            return
                principal.IsInRole(ManagerRole.NormalizedName) ||
                principal.IsInRole(AdministratorRole.NormalizedName);
        }

        public static bool IsAdmin(this ClaimsPrincipal? principal)
        {
            if (principal == null) return false;
            return principal.IsInRole(AdministratorRole.NormalizedName);
        }
    }
}
