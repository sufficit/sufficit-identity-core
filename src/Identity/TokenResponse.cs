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
        public string? IdToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = default!;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = default!;
    }
}
