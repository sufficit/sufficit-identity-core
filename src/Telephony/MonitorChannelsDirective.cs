using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class MonitorChannelsDirective : Directive
    {
        public const string UniqueID = "7bc67d43-cb9a-46d9-a9eb-cc05561e0618";
        public const string RoleID = TelephonySupervisorRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "ouvir canais de áudio";

        public override string Key => "monitorchannels";
    }
}
