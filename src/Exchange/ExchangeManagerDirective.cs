using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Exchange
{
    public class ExchangeManagerDirective : Directive
    {
        public const string UniqueID = "2c45c5ac-f6g6-6d86-bahc-1ed8c6ccc83h";

        public const string RoleID = ExchangeManagerRole.UniqueID;

        public const string NormalizedKey = ExchangeManagerRole.NormalizedName;

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso a administração de exchange";

        public override string Key => NormalizedKey;
    }
}
