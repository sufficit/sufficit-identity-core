using Sufficit.Identity;
using System;

namespace Sufficit.Sales
{

/// <summary>Read commercial transport evidence in assigned customer contexts; grants no retry or operational rights.</summary>
public sealed class SalesIntegrationAuditEntitlement : Entitlement
{
    /// <summary>Stable persisted entitlement identity.</summary>
    public const string UniqueID = "e578b7d796924f09846955f619bb45aa";
    /// <summary>Stable token and policy assignment key.</summary>
    public const string NormalizedKey = "salesintegrationaudit";
    /// <summary>Persisted identity used by permission assignments.</summary>
    public override Guid ID => Guid.Parse(UniqueID);
    /// <summary>This scoped grant never derives a platform role.</summary>
    public override Guid IDRole => Guid.Empty;
    /// <summary>Default user-facing label in Brazilian Portuguese.</summary>
    public override string Name => "consultar auditoria de integrações";
    /// <summary>Stable serialized policy key.</summary>
    public override string Key => NormalizedKey;
}

}
