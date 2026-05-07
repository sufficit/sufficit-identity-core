using Sufficit.Identity;
using System;

namespace Sufficit.AI
{
    /// <summary>
    /// Grants a user the right to consume the AI module: create API tokens, authenticate
    /// requests against AI models, and use the inference API.
    /// </summary>
    /// <remarks>
    /// Context semantics: the IDContext on assignment should be the customer/company context
    /// the user belongs to. Use IDContext = Guid.Empty to grant access across all contexts.
    /// Verification: use HasPolicy(principal, new AIUserDirective()) to check possession
    /// on any context (typical case), or HasPolicy&lt;AIUserDirective&gt;(principal, contextId)
    /// to scope the check to a specific customer context.
    /// This directive governs consumption only. For module administration (providers, presets,
    /// configuration), see AIControlDirective.
    /// </remarks>
    public class AIUserDirective : Directive
    {
        public const string UniqueID = "12aca70d-1b97-4b4c-9029-5d0eb7db3260";

        public const string NormalizedKey = "aiuser";

        public override Guid ID => Guid.Parse(UniqueID);

        // No role association — directive possession alone grants access
        public override Guid IDRole => Guid.Empty;

        public override string Name => "uso do aplicativo de inteligência artificial";

        public override string Key => NormalizedKey;
    }
}
