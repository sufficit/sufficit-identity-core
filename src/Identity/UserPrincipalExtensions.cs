﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Sufficit.Identity
{
    public static class UserPrincipalExtensions
    {
        #region IS IN ROLE

        public static bool IsInRole(this UserPrincipal? source, IRole role) 
            => source?.Roles.Contains(role) ?? false;

        public static bool IsInRole<T>(this UserPrincipal? source) where T : IRole, new() 
            => source?.Roles.Contains(new T()) ?? false;

        public static bool IsInRole(this UserPrincipal? source, IEnumerable<IRole> roles)
        {
            if (source != null)
            {
                foreach (var role in roles)
                    if (source.IsInRole(role)) return true;
            }

            return false;
        }

        public static bool IsInRole(this UserPrincipal? source, Type[] roles) 
            => source?.Roles.Any(s => roles.Contains(s.GetType())) ?? false;

        public static bool IsInRole<T, U>(this UserPrincipal? source) where T : IRole 
            => source?.IsInRole(new[] { typeof(T), typeof(U) }) ?? false;
        
        #endregion

        /// <summary>
        /// Get user Guid ID from ClaimTypes.UserID
        /// </summary>
        /// <returns></returns>
        public static Guid GetUserId(this UserPrincipal? source)
            => source is ClaimsPrincipal principal ? principal.GetUserId() : Guid.Empty;
    }
}
