using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sufficit.Identity
{
    public interface ITokenProvider
    {
        ValueTask<string?> GetTokenAsync();
    }
}
