using System;
namespace Sufficit.Sales
{

/// <summary>Current API decision for one context; not a grant or a replacement for endpoint authorization.</summary>
public sealed class SalesIntegrationAccessState
{
    /// <summary>Explicit customer context whose permissions were evaluated.</summary>
    public Guid ContextId { get; set; }
    /// <summary>Whether current trusted authorization allows audit queries in this context.</summary>
    public bool CanRead { get; set; }
    /// <summary>Whether current trusted authorization and requester identity allow retry requests in this context.</summary>
    public bool CanRetry { get; set; }
}

}
