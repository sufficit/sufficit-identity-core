using System;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Response model for OAuth 2.0 Token Introspection (RFC 7662)
    /// </summary>
    public class TokenIntrospectionResponse
    {
        /// <summary>
        /// Boolean indicator of whether or not the presented token is currently active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// A JSON string containing a space-separated list of scopes associated with this token
        /// </summary>
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        /// <summary>
        /// Client identifier for the OAuth 2.0 client that the token was issued to
        /// </summary>
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }

        /// <summary>
        /// Human-readable identifier for the resource owner who authorized this token
        /// </summary>
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Type of the token (e.g., "Bearer")
        /// </summary>
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        /// <summary>
        /// Integer timestamp indicating when this token will expire
        /// </summary>
        [JsonPropertyName("exp")]
        public long? Expiration { get; set; }

        /// <summary>
        /// Integer timestamp indicating when this token was issued
        /// </summary>
        [JsonPropertyName("iat")]
        public long? IssuedAt { get; set; }

        /// <summary>
        /// Integer timestamp indicating when this token is not to be used before
        /// </summary>
        [JsonPropertyName("nbf")]
        public long? NotBefore { get; set; }

        /// <summary>
        /// Subject of the token (usually a machine-readable identifier of the resource owner)
        /// </summary>
        [JsonPropertyName("sub")]
        public string? Subject { get; set; }

        /// <summary>
        /// Service-specific string identifier or list of string identifiers representing the intended audience.
        ///
        /// The real introspection payload from Sufficit Identity can emit <c>aud</c> either as a single string
        /// or as an array when the same reference token is valid for more than one resource. We store the raw
        /// contract as an array so token validation UIs and downstream callers keep working in both cases.
        /// </summary>
        [JsonPropertyName("aud")]
        [JsonConverter(typeof(StringOrStringArrayJsonConverter))]
        public string[] Audiences { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Backward-compatible shortcut for legacy callers that still expect a single audience string.
        /// When multiple audiences exist, the first one is the canonical primary resource.
        /// </summary>
        [JsonIgnore]
        public string? Audience
        {
            get => Audiences.Length > 0 ? Audiences[0] : null;
            set => Audiences = string.IsNullOrWhiteSpace(value) ? Array.Empty<string>() : new[] { value };
        }

        /// <summary>
        /// String representing the issuer of this token
        /// </summary>
        [JsonPropertyName("iss")]
        public string? Issuer { get; set; }

        /// <summary>
        /// String identifier for the token
        /// </summary>
        [JsonPropertyName("jti")]
        public string? JwtId { get; set; }

        // Convenience properties for easier access
        /// <summary>
        /// Expiration date and time (converted from Expiration timestamp)
        /// </summary>
        public DateTime? ExpirationDateTime => Expiration.HasValue
            ? (DateTime?)DateTimeOffset.FromUnixTimeSeconds(Expiration.Value).DateTime
            : null;

        /// <summary>
        /// Issued at date and time (converted from IssuedAt timestamp)
        /// </summary>
        public DateTime? IssuedAtDateTime => IssuedAt.HasValue
            ? (DateTime?)DateTimeOffset.FromUnixTimeSeconds(IssuedAt.Value).DateTime
            : null;

        /// <summary>
        /// Not before date and time (converted from NotBefore timestamp)
        /// </summary>
        public DateTime? NotBeforeDateTime => NotBefore.HasValue
            ? (DateTime?)DateTimeOffset.FromUnixTimeSeconds(NotBefore.Value).DateTime
            : null;

        /// <summary>
        /// Whether the token is expired
        /// </summary>
        public bool IsExpired => ExpirationDateTime.HasValue && ExpirationDateTime.Value < DateTime.UtcNow;

        /// <summary>
        /// Split scopes into an array
        /// </summary>
        public string[] Scopes => string.IsNullOrWhiteSpace(Scope) 
            ? new string[0] 
            : Scope.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
