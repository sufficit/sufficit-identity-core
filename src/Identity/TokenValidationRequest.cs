using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Request model for token validation
    /// </summary>
    public class TokenValidationRequest
    {
        /// <summary>
        /// The token to be validated
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// OPTIONAL hint about the type of the token (RFC 7662 §2.1), e.g. <c>access_token</c> or
        /// <c>refresh_token</c>. A server that misses with the hint MUST extend its search across
        /// all supported token types, so omitting it is always legal — only slower.
        /// </summary>
        [JsonPropertyName("token_type_hint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenTypeHint { get; set; }
    }
}
