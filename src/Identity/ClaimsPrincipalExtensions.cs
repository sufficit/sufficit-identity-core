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
                foreach (var identity in source.Identities)
                    foreach (var claim in identity.Claims)
                        yield return claim;
        }

        /// <summary>
        ///     Shortcut for Principal.Identity - IsAuthenticated
        /// </summary>
        public static bool IsAuthenticated(this ClaimsPrincipal? source)
            => source?.Identity?.IsAuthenticated ?? false;
            
        /// <summary>
        ///     Empty ContextId means that user has rights on all contexts ids
        /// </summary>
        public static bool HasPolicy<T>(this ClaimsPrincipal principal, Guid contextid) where T : IDirective
        {
            foreach (var userDirective in GetUserPolicies(principal))
            {
                if (userDirective.Directive is T && (userDirective.IDContext == contextid || userDirective.IDContext == Guid.Empty))
                    return true;
            }
            return false;
        }

        /// <inheritdoc cref="HasDirective"/>
        public static IEnumerable<Guid> HasDirective<T>(this ClaimsPrincipal principal) where T : IDirective, new()
            => principal.HasDirective(new T());

        /// <inheritdoc cref="HasDirective"/>
        public static bool HasDirective<T>(this ClaimsPrincipal principal, Guid context) where T : IDirective, new()
            => principal.HasDirective(new T()).Any(s => s == context);

        /// <summary>
        ///     Empty ContextId means that user has rights on all contexts ids
        /// </summary>
        public static IEnumerable<Guid> HasDirective(this ClaimsPrincipal principal, IDirective directive)
        {
            var items = new HashSet<Guid>();
            foreach (var userDirective in GetUserPolicies(principal))
            {
                // old method does not brings empty contexts ??? dont known why its was removed
                // if (userDirective.Directive.Equals(directive) && userDirective.IDContext != Guid.Empty)
                if (userDirective.Directive.Equals(directive))
                    items.Add(userDirective.IDContext);
            }
            return items;
        }

        /// <summary>
        /// Indicates that a Principal have a directive on any context
        /// </summary>
        /// <returns></returns>
        public static bool HasPolicy(this ClaimsPrincipal principal, IDirective directive)
        {
            foreach (var userDirective in GetUserPolicies(principal))
            {
                if (userDirective.Directive.Equals(directive))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// User contexts that requested directives exists
        /// </summary>
        public static IEnumerable<UserPolicy> GetUserPolicies(this ClaimsPrincipal principal)
        {
            foreach (var claim in principal.Claims.Where(s => s.Type == ClaimTypes.Directive))
                yield return claim.ToUserPolicy();
        }

        /// <summary>
        /// Get all contexts that user has policies for
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static IEnumerable<Guid> KnowingContexts(this ClaimsPrincipal principal)
        {
            HashSet<Guid> contexts = new HashSet<Guid>();
            foreach (var contextId in principal.GetUserPolicies().Select(s => s.IDContext))
            {
                if (contextId != Guid.Empty)
                    contexts.Add(contextId);
            }
            return contexts;
        }
        
        /// <summary>
        /// Check if the principal knows about a specific context
        /// This is useful to determine if the user has any policies related to the context
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public static bool IsKnowingContext(this ClaimsPrincipal principal, Guid contextId)
            => principal.KnowingContexts().Contains(contextId);
    }
}
