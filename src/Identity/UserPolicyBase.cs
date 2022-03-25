using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sufficit.Identity
{
    [DataContract]
    public class UserPolicyBase
    {
        /// <summary>
        /// To deserializer
        /// </summary>
        public UserPolicyBase() { }

        public UserPolicyBase(Guid directive, Guid context)
        {
            IDDirective     = directive;
            IDContext       = context;
        }

        [DataMember(Name = "iddirective")]
        public Guid IDDirective { get; set; }

        [DataMember(Name = "idcontext")]
        public Guid IDContext { get; set; }

        public override bool Equals(object other) =>
           other is UserPolicyBase p && (p.IDContext, p.IDDirective).Equals((IDContext, IDDirective));

        public override int GetHashCode() => 
            (IDContext, IDDirective).GetHashCode();
    }
}
