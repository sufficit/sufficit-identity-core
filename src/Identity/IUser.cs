using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public interface IUser
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// String representation, Name, Username or E-Mail
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Authorization directives and contexts
        /// </summary>
        IEnumerable<UserPolicy> Policies { get; }

        /// <summary>
        /// Resume of roles from user policies
        /// </summary>
        IEnumerable<IRole> Roles { get; }

        bool IsInRole(string roleName); 
    }
}
