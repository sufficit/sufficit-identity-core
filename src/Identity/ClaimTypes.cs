using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public static class ClaimTypes
    {
        public const string MicrosoftNameIdentifier = System.Security.Claims.ClaimTypes.NameIdentifier;
        public const string MicrosoftRole = System.Security.Claims.ClaimTypes.Role;

        /// <summary>
        /// Define user roles
        /// </summary>
        public const string Role = "role";

        /// <summary>
        /// Define user directives policies
        /// </summary>
        public const string Directive = "directive";

        /// <summary>
        /// Define user id in GUID format
        /// </summary>
        public const string UserID = "sub";

        /// <summary>
        /// Define user full name or name representation
        /// </summary>
        public const string UserFullName = System.Security.Claims.ClaimTypes.Name;
    }
}
