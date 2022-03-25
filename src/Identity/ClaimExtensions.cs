using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Sufficit.Telephony;

namespace Sufficit.Identity
{
    public static class ClaimExtensions
    {   
        public static UserPolicy ToUserPolicy(this System.Security.Claims.Claim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            if (claim.Type != ClaimTypes.Directive) throw new Exception($"invalid claim type: { claim.Type }");
            if (string.IsNullOrWhiteSpace(claim.Value)) throw new Exception("empty claim value");

            var claimType = claim.Value.Split(':');
            if (claimType.Length != 2) throw new Exception($"invalid claim type: { claim.Value }");

            return UserPolicy.Generate(claimType[0], claimType[1]);
        }        
    }
}
