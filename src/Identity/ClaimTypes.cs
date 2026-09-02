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
        /// <remarks>
        ///     Short historical name: no namespace, and absent from the IANA JWT
        ///     claim registry, which RFC 7519 section 4.3 advises against for
        ///     private claims. Kept because consumers still read it; see
        ///     <see cref="Entitlement"/> for the replacement and
        ///     docs/decisions/0001-entitlement-naming.md for the migration order.
        /// </remarks>
        public const string Directive = "directive";

        /// <summary>
        ///     Standard container for authorization grants: RFC 9068 section
        ///     2.2.3.2, with SCIM semantics from RFC 7643 section 4.1.2.
        /// </summary>
        /// <remarks>
        ///     Emitted alongside <see cref="Directive"/> during the transition.
        ///     Readers accept both; producers stop emitting the short name only
        ///     after the last consumer has migrated, never before.
        /// </remarks>
        public const string Entitlement = "entitlements";

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
