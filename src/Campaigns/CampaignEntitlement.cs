using Sufficit.Identity;
using System;

namespace Sufficit.Campaigns
{
    /// <summary>
    ///     Manage and follow the outreach campaigns of a context (API and real-time events)
    /// </summary>
    public class CampaignEntitlement : Entitlement
    {
        public const string UniqueID = "5d1f41cb9ca6481b888aba9340e6a557";
        public const string RoleID = CampaignManagerRole.UniqueID;

        public const string NormalizedKey = "campaigns";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "gerenciar campanhas";

        public override string Key => NormalizedKey;
    }
}
