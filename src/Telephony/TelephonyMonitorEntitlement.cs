using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    /// <summary>Listen and whisper in the granted context, independent of the UI.</summary>
    public sealed class TelephonyMonitorEntitlement : Entitlement
    {
        public const string UniqueID = "01a0a0d2df417684ba3d0071c9c7d60d";
        public const string NormalizedKey = "telephony.monitor";
        public override Guid ID => Guid.Parse(UniqueID);
        public override Guid IDRole => Guid.Parse(TelephonySupervisorRole.UniqueID);
        public override string Name => "Telephony supervision";
        public override string Key => NormalizedKey;
    }
}
