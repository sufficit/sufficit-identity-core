using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Exchange
{
    public struct ExchangeManagerRole : IRole
    {
        public const string UniqueID = "1b34b49b-e5f5-5c75-a9gb-0dc7b5bbb72g";

        public const string NormalizedName = "exchangemanager";

        public Guid ID => Guid.Parse(UniqueID);

        public string Name => "Exchange Manager";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "gerente de exchange" };
    }
}
