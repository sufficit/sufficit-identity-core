using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    /// <summary>
    ///     Comes with the dial plan entitlement, usually for one company, so customers hold it.
    ///     It is not internal and never grants access across customers (the "manager" role is the internal one).
    /// </summary>
    public struct TelephonyDialPlanEditorRole : IRole
    {
        public const string UniqueID = "e9099691e1c7498ab3d6364d01582176";

        public const string NormalizedName = "telephony.dialplan.editor";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Telephony Dial Plan Editor";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "editor do plano de discagem" };
    }
}
