using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sufficit.Identity
{
    /// <summary>
    /// Is an IDirective applied to an Context (ID)
    /// </summary>
    public class UserPolicy : UserPolicyBase
    {
        public UserPolicy (Guid idcontext, IDirective directive) : base(directive.ID, idcontext)
        {
            Directive = directive; 
        }

        public IDirective Directive { get; }

        public override bool Equals (object? other) =>
            base.Equals(other);

        public override int GetHashCode () => 
            base.GetHashCode();
    }
}
