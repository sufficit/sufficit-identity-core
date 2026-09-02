using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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
        ///     Empty ContextId means that user has rights on all contexts ids, except for
        ///     self-context entitlements, where it means the authenticated user's own context.
        /// </summary>
        public static bool HasPolicy<T>(this ClaimsPrincipal principal, Guid contextid) where T : IEntitlement
        {
            foreach (var userEntitlement in GetUserPolicies(principal))
            {
                if (userEntitlement.Entitlement is T && MatchesContext(principal, userEntitlement, contextid))
                    return true;
            }
            return false;
        }

        /// <inheritdoc cref="HasEntitlement"/>
        public static IEnumerable<Guid> HasEntitlement<T>(this ClaimsPrincipal principal) where T : IEntitlement, new()
            => principal.HasEntitlement(new T());

        /// <inheritdoc cref="HasEntitlement"/>
        public static bool HasEntitlement<T>(this ClaimsPrincipal principal, Guid context) where T : IEntitlement, new()
            => principal.HasEntitlement(new T()).Any(s => s == context);

        /// <summary>
        ///     Returns effective contexts. For self-context entitlements, an empty stored
        ///     context is resolved to the authenticated user's own ID.
        /// </summary>
        public static IEnumerable<Guid> HasEntitlement(this ClaimsPrincipal principal, IEntitlement entitlement)
        {
            var items = new HashSet<Guid>();
            foreach (var userEntitlement in GetUserPolicies(principal))
            {
                if (userEntitlement.Entitlement.Equals(entitlement))
                {
                    var context = GetEffectiveContext(principal, userEntitlement);
                    if (context.HasValue)
                        items.Add(context.Value);
                }
            }
            return items;
        }

        /// <summary>
        /// Indicates that a Principal have a entitlement on any context
        /// </summary>
        /// <returns></returns>
        public static bool HasPolicy(this ClaimsPrincipal principal, IEntitlement entitlement)
        {
            foreach (var userEntitlement in GetUserPolicies(principal))
            {
                if (userEntitlement.Entitlement.Equals(entitlement) &&
                    GetEffectiveContext(principal, userEntitlement).HasValue)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// User contexts that requested entitlements exists.
        /// Handles both individual entitlement claims and JSON-array encoded claims
        /// (where the identity provider packs multiple entitlements into a single claim value).
        /// </summary>
        public static IEnumerable<UserPolicy> GetUserPolicies(this ClaimsPrincipal principal)
            => GetUserPolicies(principal, null);

        /// <summary>
        /// User contexts that requested entitlements exists, logging and ignoring malformed
        /// or unknown entitlement claims so one product-specific claim cannot reject the user.
        /// </summary>
        public static IEnumerable<UserPolicy> GetUserPolicies(this ClaimsPrincipal principal, ILogger? logger)
        {
            foreach (var claim in principal.Claims.Where(s =>
                s.Type == ClaimTypes.Directive || s.Type == ClaimTypes.Entitlement))
            {
                if (string.IsNullOrWhiteSpace(claim.Value)) continue;

                var trimmed = claim.Value.TrimStart();

                // JSON array: identity provider encoded multiple entitlements as a single claim
#if NETSTANDARD2_0
                if (trimmed.StartsWith("["))
#else
                if (trimmed.StartsWith('['))
#endif
                {
                    IEnumerable<string> entries = System.Array.Empty<string>();
                    try
                    {
                        var deserialized = JsonSerializer.Deserialize<IEnumerable<string>>(trimmed);
                        if (deserialized != null) entries = deserialized;
                    }
                    catch (Exception ex)
                    {
                        LogEntitlementWarning(logger, claim.Value, ex.GetType().Name);
                    }

                    foreach (var entry in entries)
                    {
                        if (string.IsNullOrWhiteSpace(entry)) continue;
                        if (!entry.Contains(':')) continue;
                        var arrayClaim = new Claim(claim.Type, entry, claim.ValueType, claim.Issuer);
                        var parsed = TryParseUserPolicy(arrayClaim, logger);
                        if (parsed != null) yield return parsed;
                    }
                    continue;
                }

                // Plain object or other JSON — skip
#if NETSTANDARD2_0
                if (trimmed.StartsWith("{")) continue;
#else
                if (trimmed.StartsWith('{')) continue;
#endif

                // Plain scalar entitlement value
                if (!claim.Value.Contains(':')) continue;
                var policy = TryParseUserPolicy(claim, logger);
                if (policy != null) yield return policy;
            }
        }

        private static
#nullable enable
            UserPolicy?
#nullable restore
            TryParseUserPolicy(Claim claim, ILogger? logger)
        {
            try { return claim.ToUserPolicy(); }
            catch (Exception ex)
            {
                LogEntitlementWarning(logger, claim.Value, GetFailureReason(ex));
                return null;
            }
        }

        private static string GetFailureReason(Exception exception)
        {
            if (exception is ArgumentException argumentException)
            {
                if (argumentException.ParamName == "key") return "UnknownEntitlement";
                if (argumentException.ParamName == "context") return "InvalidContext";
            }

            return exception.GetType().Name;
        }

        private static void LogEntitlementWarning(ILogger? logger, string value, string reason)
        {
            if (logger == null) return;

            logger.LogWarning(
                "Ignoring invalid or unknown entitlement claim. EntitlementKey={EntitlementKey} Reason={Reason}",
                GetEntitlementKeyForLog(value),
                reason);
        }

        private static string GetEntitlementKeyForLog(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "<empty>";

            var trimmed = value.TrimStart();
#if NETSTANDARD2_0
            if (trimmed.StartsWith("[")) return "<json-array>";
            if (trimmed.StartsWith("{")) return "<json-object>";
#else
            if (trimmed.StartsWith('[')) return "<json-array>";
            if (trimmed.StartsWith('{')) return "<json-object>";
#endif

            var separatorIndex = value.IndexOf(':');
            var key = (separatorIndex > 0 ? value.Substring(0, separatorIndex) : value).Trim();
            if (key.Length == 0) return "<empty>";

            return new string(key
                .Take(64)
                .Select(character => char.IsLetterOrDigit(character) || character == '-' || character == '_' || character == '.'
                    ? character
                    : '_')
                .ToArray());
        }

        /// <summary>
        /// Get all contexts that user has policies for
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static IEnumerable<Guid> KnowingContexts(this ClaimsPrincipal principal)
        {
            HashSet<Guid> contexts = new HashSet<Guid>();
            foreach (var policy in principal.GetUserPolicies())
            {
                var contextId = GetEffectiveContext(principal, policy);
                if (contextId.HasValue && contextId.Value != Guid.Empty)
                    contexts.Add(contextId.Value);
            }
            return contexts;
        }

        private static bool MatchesContext(ClaimsPrincipal principal, UserPolicy policy, Guid contextId)
        {
            if (policy.IDContext != Guid.Empty)
                return policy.IDContext == contextId;

            if (policy.Entitlement is not ISelfContextEntitlement)
                return true;

            var userId = principal.GetUserId();
            return userId != Guid.Empty && userId == contextId;
        }

        private static Guid? GetEffectiveContext(ClaimsPrincipal principal, UserPolicy policy)
        {
            if (policy.IDContext != Guid.Empty)
                return policy.IDContext;

            if (policy.Entitlement is not ISelfContextEntitlement)
                return Guid.Empty;

            var userId = principal.GetUserId();
            return userId != Guid.Empty ? userId : (Guid?)null;
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
            
        public static IEnumerable<Claim> GetRoles(this ClaimsPrincipal? principal)
        {
            var roles = new HashSet<Claim>();
            if (principal == null) return roles;

            foreach (var claim in principal.Claims.Where(s => s.Type == ClaimTypes.Role || s.Type == ClaimTypes.MicrosoftRole))            
                roles.Add(claim);
            
            return roles.GroupBy(s => s.Value).Select(g => g.First());
        }
    }
}
