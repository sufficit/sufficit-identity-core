using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sufficit.Identity
{
    /// <summary>
    ///     Entitlement vocabulary over the existing checks.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The published name for a grant is <em>entitlement</em>: the
    ///         container RFC 9068 section 2.2.3.2 defines, with SCIM semantics
    ///         from RFC 7643 section 4.1.2. New code should read this way.
    ///     </para>
    ///     <para>
    ///         These are aliases, not a second implementation. The concrete
    ///         types are still named <c>*Directive</c> because entitlement
    ///         discovery enumerates every <see cref="IDirective"/> through
    ///         reflection and <c>HasPolicy&lt;T&gt;</c> matches by type — a
    ///         compatibility subclass would be discovered as a second, separate
    ///         entitlement and would silently fail type checks. Renaming the
    ///         types is a coordinated pass across consumers; see
    ///         docs/decisions/0001-entitlement-naming.md.
    ///     </para>
    /// </remarks>
    public static class EntitlementExtensions
    {
        /// <summary>
        ///     Contexts where the principal holds this entitlement.
        /// </summary>
        public static IEnumerable<Guid> HasEntitlement<T>(this ClaimsPrincipal principal)
            where T : IDirective, new()
            => principal.HasDirective<T>();

        /// <summary>
        ///     Whether the principal holds this entitlement in this context.
        /// </summary>
        /// <remarks>
        ///     The context is always explicit: a principal may hold different
        ///     entitlements in different contexts at the same time, and there is
        ///     no ambient current context to fall back on.
        /// </remarks>
        public static bool HasEntitlement<T>(this ClaimsPrincipal principal, Guid context)
            where T : IDirective, new()
            => principal.HasDirective<T>(context);

        /// <inheritdoc cref="HasEntitlement{T}(ClaimsPrincipal)"/>
        public static IEnumerable<Guid> HasEntitlement(
            this ClaimsPrincipal principal, IDirective entitlement)
            => principal.HasDirective(entitlement);
    }
}
