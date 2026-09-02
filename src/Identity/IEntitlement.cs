using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Basic interface to define rights entitlements, allow or disallow
    /// </summary>
    public interface IEntitlement
    {
        Guid ID { get; }
        Guid IDRole { get; }
        string Name { get; }        
        string? Description { get; }
        string Key { get; }
    }
}
