using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    /// <summary>
    ///     Comes with the portability entitlement, usually for one company, so customers hold it.
    ///     It is not internal and never grants access across customers (the "manager" role is the internal one).
    /// </summary>
    public struct TelephonyPortabilityEditorRole : IRole
    {
        public const string UniqueID = "f5eacb1050b6436f9a3f0432e896ece6";

        public const string NormalizedName = "telephony.portability.editor";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Telephony Portability Editor";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "editor de portabilidade" };
    }
}
