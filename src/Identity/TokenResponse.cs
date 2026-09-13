using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Represents the response received from an authentication server when requesting an access token.
    /// </summary>
    /// <remarks>This class encapsulates the details of a token response, including the access token, its
    /// expiration time, the token type, the associated scope, and an optional refresh token. It is typically used in
    /// OAuth2 or similar authentication flows.</remarks>
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("id_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IdToken { get; set; }

        /// <summary>RECOMMENDED but omittable (RFC 6749 §5.1) — the lifetime in seconds of the access token. Null when the server omits it; a consumer must not read it as "expires immediately" (0).</summary>
        [JsonPropertyName("expires_in")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;

        /// <summary>OPTIONAL when identical to the scope requested by the client; REQUIRED when it differs (RFC 6749 §5.1). Null when the server omits it.</summary>
        [JsonPropertyName("scope")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Scope { get; set; }

        /// <summary>OPTIONAL per RFC 6749 §5.1 — null when not issued (e.g. the client-credentials grant, §4.4.3, SHOULD NOT include one).</summary>
        [JsonPropertyName("refresh_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RefreshToken { get; set; }
    }
}
