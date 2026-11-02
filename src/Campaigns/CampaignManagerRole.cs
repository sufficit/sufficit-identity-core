using Sufficit.Identity;
using System;

namespace Sufficit.Campaigns
{
    public struct CampaignManagerRole : IRole
    {
        public const string UniqueID = "f742d92cb2c7420b8a2ffb0f83436ae1";

        public const string NormalizedName = "campaignmanager";

        public readonly Guid ID => Guid.Parse(UniqueID);

        public readonly string Name => "Campaign Manager";

        string IRole.NormalizedName => NormalizedName;

        string[] IRole.Filter => new[] { NormalizedName, "gerente de campanhas" };
    }
}
