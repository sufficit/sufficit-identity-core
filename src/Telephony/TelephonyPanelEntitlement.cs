using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    /// <summary>View operational telephony data in the granted context.</summary>
    public sealed class TelephonyPanelEntitlement : Entitlement
    {
        public const string UniqueID = "01a0a0d2df417684ba3d007070e11d20";
        public const string NormalizedKey = "telephony.panel";
        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(TelephonyRole.UniqueID);
        public override string Name => "Telephony panel";
        public override string Key => NormalizedKey;
    }
}
