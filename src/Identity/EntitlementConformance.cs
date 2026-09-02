using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sufficit.Identity
{
    /// <summary>
    ///     A shared set of authorization decisions every application must agree on.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Separate tokens and separate scopes already stop one application from
    ///         using another's credentials. They do nothing about a different risk:
    ///         two applications reading the <em>same</em> grant and reaching
    ///         <em>different</em> decisions. The data is shared even when the
    ///         credentials are not.
    ///     </para>
    ///     <para>
    ///         Divergence does not announce itself. An application that grants too
    ///         much renders a working screen; nobody reports being able to see more
    ///         than they should. So the check has to be mechanical: the same inputs,
    ///         the same expected answers, executed inside each application's own test
    ///         suite against its own resolved dependencies.
    ///     </para>
    ///     <para>
    ///         Cases live here rather than in any single application so that adding
    ///         one obliges every consumer to answer it.
    ///     </para>
    /// </remarks>
    public static class EntitlementConformance
    {
        /// <summary>Deliberately fake, and never a real customer context.</summary>
        public const string ContextA = "11111111-1111-1111-1111-111111111111";

        /// <summary>A second context, to prove one grant does not leak into another.</summary>
        public const string ContextB = "22222222-2222-2222-2222-222222222222";

        /// <summary>One decision every application must reach identically.</summary>
        public sealed class Case
        {
            public Case(
                string description,
                string claimType,
                string claimValue,
                string context,
                bool expected)
            {
                Description = description;
                ClaimType = claimType;
                ClaimValue = claimValue;
                Context = context;
                Expected = expected;
            }

            public string Description { get; }

            public string ClaimType { get; }

            public string ClaimValue { get; }

            public string Context { get; }

            public bool Expected { get; }

            /// <summary>Builds the principal this case describes.</summary>
            public ClaimsPrincipal Principal() =>
                new ClaimsPrincipal(new ClaimsIdentity(
                    new[] { new Claim(ClaimType, ClaimValue) },
                    "conformance"));

            public override string ToString() => Description;
        }

        /// <summary>
        ///     The cases, keyed on the telephony entitlement because every product
        ///     can reference it without depending on another product's vocabulary.
        /// </summary>
        public static IReadOnlyList<Case> Cases { get; } = new[]
        {
            new Case(
                "standard container is honoured",
                ClaimTypes.Entitlement, $"phonecalls:{ContextA}", ContextA, true),

            new Case(
                "historical claim name is still honoured",
                ClaimTypes.Directive, $"phonecalls:{ContextA}", ContextA, true),

            // The same value written both ways is equal as a Guid and unequal as
            // text. An application comparing raw strings passes the case above and
            // fails this one — which is the whole point of it.
            new Case(
                "compact context spelling resolves to the same context",
                ClaimTypes.Entitlement,
                $"phonecalls:{ContextA.Replace("-", string.Empty)}", ContextA, true),

            new Case(
                "a grant in another context does not apply here",
                ClaimTypes.Entitlement, $"phonecalls:{ContextA}", ContextB, false),

            new Case(
                "a different entitlement in this context does not apply",
                ClaimTypes.Entitlement, $"audioadmin:{ContextA}", ContextA, false),

            // A value with whitespace would become two entitlements wherever a
            // space-separated list is assumed. It must grant nothing, everywhere.
            new Case(
                "a value with whitespace grants nothing",
                ClaimTypes.Entitlement,
                $"phonecalls:{ContextA} audioadmin:{ContextA}", ContextA, false),

            new Case(
                "an unparsable context grants nothing",
                ClaimTypes.Entitlement, "phonecalls:not-a-guid", ContextA, false),

            new Case(
                "an unknown entitlement key grants nothing",
                ClaimTypes.Entitlement, $"nao-existe:{ContextA}", ContextA, false),

            new Case(
                "an empty value grants nothing",
                ClaimTypes.Entitlement, string.Empty, ContextA, false),

            new Case(
                "a value without a separator grants nothing",
                ClaimTypes.Entitlement, "phonecalls", ContextA, false),
        };
    }
}
