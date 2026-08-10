using System;
using Sufficit.Identity;

namespace Sufficit.Cloud
{
    /// <summary>
    /// Grants regular-user access to the Cloud Mobile tenant represented by
    /// the context GUID in <c>mobile:&lt;contextId&gt;</c>.
    /// </summary>
    public sealed class MobileDirective : Directive
    {
        public const string UniqueID = "f0a8bba0-56b5-4d18-bf48-2f49c9f7e3c1";
        public const string NormalizedKey = "mobile";

        public override Guid ID => Guid.Parse(UniqueID);

        public override string Name => "acesso ao Cloud Mobile";

        public override string Key => NormalizedKey;
    }
}
