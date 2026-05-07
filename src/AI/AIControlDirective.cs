using Sufficit.Identity;
using System;

namespace Sufficit.AI
{
    /// <summary>
    /// Grants access to the AI module control interface (providers, presets, configuration).
    /// Intended for users who need to administer AI resources without holding a global system role.
    /// </summary>
    /// <remarks>
    /// Context semantics: the IDContext on assignment should be the customer/company context
    /// the user will manage. Use IDContext = Guid.Empty to grant access across all contexts.
    /// Verification: use HasPolicy(principal, new AIControlDirective()) to check possession
    /// on any context, or HasPolicy&lt;AIControlDirective&gt;(principal, contextId) to scope
    /// the check to a specific context.
    /// </remarks>
    public class AIControlDirective : Directive
    {
        public const string UniqueID = "43c12678-e2aa-46d3-bfa7-0b71b690be4d";

        public const string NormalizedKey = "aicontrol";

        public override Guid ID => Guid.Parse(UniqueID);

        // No role association — directive possession alone grants management access
        public override Guid IDRole => Guid.Empty;

        public override string Name => "controle do aplicativo de inteligência artificial";

        public override string Key => NormalizedKey;
    }
}
