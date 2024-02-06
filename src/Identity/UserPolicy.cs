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
        public UserPolicy(Guid idcontext, IDirective directive) : base(directive.ID, idcontext)
        {
            Directive = directive; 
        }

        public IDirective Directive { get; }

        public override bool Equals(object? other) =>
            base.Equals(other);

        public override int GetHashCode() => 
            base.GetHashCode();

        public static UserPolicy Generate(string key, string context)
        {
            if (!Guid.TryParse(context, out Guid IDContext)) throw new ArgumentException($"invalid context guid format: {context}", nameof(context));
            var directive = Sufficit.Identity.Directive.Enumerator.FirstOrDefault(s => s.Key == key);
            if (directive == null) throw new ArgumentException($"directive key not found: {key}", nameof(key));

            return new UserPolicy(IDContext, directive);
        }
    }
}
