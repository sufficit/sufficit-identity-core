using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    /// <summary>
    /// Usado nas consultas de APIs
    /// </summary>
    public class DirectiveBase : IDirective
    {
        public virtual Guid ID { get; set; }

        public virtual Guid IDRole { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Key { get; set; }
    }
}
