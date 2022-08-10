using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Sufficit.Identity
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get user Guid ID from ClaimTypes.UserID
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Guid GetUserId(this ClaimsPrincipal source)
        {
            var claim = source.Claims.FirstOrDefault(s => s.Type == ClaimTypes.UserID || s.Type == ClaimTypes.MicrosoftNameIdentifier);
            if (claim != null && Guid.TryParse(claim.Value, out Guid result)) return result;
            return Guid.Empty;
        }
    }
}
