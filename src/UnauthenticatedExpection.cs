using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit
{
    public class UnauthenticatedExpection : UnauthorizedAccessException
    {
        public UnauthenticatedExpection() : base() { }
        public UnauthenticatedExpection(string message) : base(message) { }
        public UnauthenticatedExpection(string message, Exception inner) : base(message, inner) { }
    }
}
