﻿using System;
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

            var claimType = claim.Value.Split(':');
            if (claimType.Length != 2) throw new Exception($"invalid claim type: { claim.Value }");

            return Utils.ToUserPolicy(claimType[0], claimType[1]);
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
