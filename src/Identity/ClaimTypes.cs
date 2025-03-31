using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public static class ClaimTypes
    {
        /// <inheritdoc cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
        public const string MicrosoftNameIdentifier = System.Security.Claims.ClaimTypes.NameIdentifier;

        /// <inheritdoc cref="System.Security.Claims.ClaimTypes.Role"/>
        public const string MicrosoftRole = System.Security.Claims.ClaimTypes.Role;

        /// <inheritdoc cref="System.Security.Claims.ClaimTypes.Name"/>
        public const string MicrosoftName = System.Security.Claims.ClaimTypes.Name;

        /// <summary>
        /// Define user roles (from skoruba identity)
        /// </summary>
        public const string Role = "role";


        /// <summary>
        /// Define user names (from skoruba identity)
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// Define user directives policies
        /// </summary>
        public const string Directive = "directive";

        /// <summary>
        /// Define user id in GUID format
        /// </summary>
        public const string UserID = "sub";

        /// <summary>
        ///     Default name for access token claim
        /// </summary>
        public const string AccessToken = "access_token";
    }
}
