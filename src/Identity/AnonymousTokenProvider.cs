using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Sufficit.Identity
{
    /// <summary>
    ///     Default use for access non auth methods
    /// </summary>
    public class AnonymousTokenProvider : ITokenProvider
    {
        public ValueTask<string?> GetTokenAsync()
            => new ValueTask<string?>((string?)null);        
    }
}
