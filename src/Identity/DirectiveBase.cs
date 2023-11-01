using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Usado nas consultas de APIs
    /// </summary>
    public class DirectiveBase : IDirective
    {
        public virtual Guid ID { get; set; }

        public virtual Guid IDRole { get; set; }

        public virtual string Name { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
        public virtual string? Description { get; set; }

        public virtual string Key { get; set; } = default!;
    }
}
