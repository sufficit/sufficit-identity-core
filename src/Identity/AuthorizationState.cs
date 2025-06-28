using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Represents the state of an authorization process, including a unique identifier and an optional return URL.
    /// </summary>
    public class AuthorizationState
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("returnUrl")]
        public string? ReturnUrl { get; set; }
    }
}
