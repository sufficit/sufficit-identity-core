using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class MonitorChannelsEntitlement : Entitlement
    {
        public const string UniqueID = "7bc67d43cb9a46d9a9ebcc05561e0618";
        public const string RoleID = TelephonySupervisorRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "ouvir canais de áudio";

        public override string Key => "monitorchannels";
    }
}
