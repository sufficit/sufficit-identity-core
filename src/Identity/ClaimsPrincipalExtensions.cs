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
        ///     Get user Guid ID from ClaimTypes.UserID
        /// </summary>
        public static Guid GetUserId(this ClaimsPrincipal? source)
        {
            if (source != null)
            {
                var claim = source.Claims?.FirstOrDefault(s => s.Type == ClaimTypes.UserID || s.Type == ClaimTypes.MicrosoftNameIdentifier);
                if (claim != null && Guid.TryParse(claim.Value, out Guid result)) return result;
            }
            return Guid.Empty;
        }

        /// <summary>
        ///     Get access token
        /// </summary>
        public static string? GetAccessToken(this ClaimsPrincipal? source)
        {            
            var claims = source.GetClaims();
            var claim = claims.FirstOrDefault(s => s.Type == ClaimTypes.AccessToken);
            if (claim != null && !string.IsNullOrWhiteSpace(claim.Value)) 
                return claim.Value;
          
            return null;
        }

        public static IEnumerable<Claim> GetClaims(this ClaimsPrincipal? source)
        {
            if (source != null) 
                foreach(var identity in source.Identities)            
                    foreach (var claim in identity.Claims)
                        yield return claim;
        }


        /// <summary>
        ///     Shortcut for Principal.Identity - IsAuthenticated
        /// </summary>
        public static bool IsAuthenticated (this ClaimsPrincipal? source)
            => source?.Identity?.IsAuthenticated ?? false;
    }
}
