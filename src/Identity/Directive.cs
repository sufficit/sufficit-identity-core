using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    public abstract class Directive : IDirective, IEquatable<IDirective>
    {
        public abstract Guid ID { get; }

        public virtual Guid IDRole => Guid.Empty;

        public abstract string Name { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
        public virtual string? Description { get; }

        public abstract string Key { get; }

        #region FACILITADORES
 
        public static implicit operator Guid (Directive? directive) => directive != null ? directive.ID : Guid.Empty;

        public override sealed bool Equals(object? obj)
            => obj is IDirective p && p.ID == ID;

        public override sealed int GetHashCode() 
            => ID.GetHashCode();  

        public bool Equals(IDirective? other) => this.ID == other?.ID;

        public static IEnumerable<IDirective> Enumerator { get; }
            = Utils.GetCollectionOfType<IDirective>().Where(s => !string.IsNullOrWhiteSpace(s.Key));

        #endregion
    }
}
