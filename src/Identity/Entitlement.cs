using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    public abstract class Entitlement : IEntitlement, IEquatable<IEntitlement>
    {
        public abstract Guid ID { get; }

        public virtual Guid IDRole => Guid.Empty;

        /// <summary>
        ///     Common Title for this entitlement, Use Culture Variations
        /// </summary>
        public abstract string Name { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
        public virtual string? Description { get; }

        public abstract string Key { get; }

        #region FACILITADORES
 
        public static implicit operator Guid (Entitlement? entitlement) => entitlement != null ? entitlement.ID : Guid.Empty;

        public override sealed bool Equals(object? obj)
            => obj is IEntitlement p && p.ID == ID;

        public override sealed int GetHashCode() 
            => ID.GetHashCode();  

        public bool Equals(IEntitlement? other) => this.ID == other?.ID;

        public static IEnumerable<IEntitlement> Enumerator { get; }
            = Sufficit.Utils.GetCollectionOfType<IEntitlement>().Where(s => !string.IsNullOrWhiteSpace(s.Key));

        #endregion
    }
}
