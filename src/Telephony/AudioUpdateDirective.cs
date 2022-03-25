using Sufficit.Identity;
using System;

namespace Sufficit.Telephony
{
    public class AudioUpdateDirective : Directive
    {
        public const string UniqueID = "b327fb38-66f2-4063-9cd0-da1e4241d2d5";
        public const string RoleID = TelephonyRole.UniqueID;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID); 

        public override string Name => "atualizar áudio";

        public override string Key => "audioupdate";
    }
}
