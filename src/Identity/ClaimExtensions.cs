using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sufficit.Identity
{
    public static class ClaimExtensions
    {
        /// <summary>
        /// Create a UserPolicy from (name):(guid) directive claim 
        /// </summary>
        /// <param name="claim">must be of type <see cref="ClaimTypes.Directive"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static UserPolicy ToUserPolicy(this System.Security.Claims.Claim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            if (claim.Type != ClaimTypes.Directive) throw new Exception($"invalid claim type: { claim.Type }");
            if (string.IsNullOrWhiteSpace(claim.Value)) throw new Exception("empty claim value");

            // Some identity providers encode all directives as a single JSON-array claim value.
            // Use the first ':' only so that GUID values containing colons don't break the parse,
            // and reject values that don't produce exactly two non-empty parts.
            var separatorIndex = claim.Value.IndexOf(':');
            if (separatorIndex <= 0 || separatorIndex == claim.Value.Length - 1)
                throw new Exception($"invalid claim value: { claim.Value }");

            var directiveKey = claim.Value.Substring(0, separatorIndex);
            var contextId    = claim.Value.Substring(separatorIndex + 1);

            // A JSON-array value starts with '[' — reject it as unsupported in this method.
            if (directiveKey.TrimStart().StartsWith("["))
                throw new Exception($"invalid claim value: { claim.Value }");

            return Utils.ToUserPolicy(directiveKey, contextId);
        }

        public static UserPolicy ToUserPolicy(this UserClaim claim)
        {
            if (claim.ClaimType != ClaimTypes.Directive) 
                throw new Exception($"invalid claim type: {claim.ClaimType}");

            if (claim.ClaimValue == null || string.IsNullOrWhiteSpace(claim.ClaimValue)) 
                throw new Exception("empty claim value");

            var claimType = claim.ClaimValue.Split(':');
            if (claimType.Length != 2) 
                throw new Exception($"invalid claim type: {claim.ClaimValue}");

            return Utils.ToUserPolicy(claimType[0], claimType[1]);
        }        
    }
}
