using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Identity
{
    public class AuthenticationUserOptions
    {        
        public string NameClaim { get; set; } = "name";

        /// <summary>
        /// Gets or sets the value to use for the System.Security.Claims.ClaimsIdentity.AuthenticationType.  
        /// </summary>    
        public string AuthenticationType { get; set; } = string.Empty;
    }
}
