using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public interface IRole
    {
        /// <summary>
        /// Unique ID of this Role
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// Easy common name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// LowerCase, no special characters, no spaces
        /// </summary>
        string NormalizedName { get; }

        /// <summary>
        /// Normalized string to compare, unique aliases
        /// </summary>
        string[] Filter { get; }
    }
}
