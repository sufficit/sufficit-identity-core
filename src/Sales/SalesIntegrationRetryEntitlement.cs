using Sufficit.Identity;
using System;

namespace Sufficit.Sales
{

/// <summary>Request a bounded transport retry in assigned contexts; grants no audit read or business command rights.</summary>
public sealed class SalesIntegrationRetryEntitlement : Entitlement
{
    /// <summary>Stable persisted entitlement identity.</summary>
    public const string UniqueID = "b928117604374e47b3630fdbe163bc2a";
    /// <summary>Stable token and policy assignment key.</summary>
    public const string NormalizedKey = "salesintegrationretry";
    /// <summary>Persisted identity used by permission assignments.</summary>
    public override Guid ID => Guid.Parse(UniqueID);
    /// <summary>This scoped grant never derives a platform role.</summary>
    public override Guid IDRole => Guid.Empty;
    /// <summary>Default user-facing label in Brazilian Portuguese.</summary>
    public override string Name => "retentar entrega de integração";
    /// <summary>Stable serialized policy key.</summary>
    public override string Key => NormalizedKey;
}

}
