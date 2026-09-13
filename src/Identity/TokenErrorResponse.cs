using System.Text.Json.Serialization;

namespace Sufficit.Identity
{
    /// <summary>
    /// Token endpoint error response (RFC 6749 §5.2), e.g.
    /// <c>{"error":"invalid_grant","error_description":"..."}</c>.
    /// </summary>
    /// <remarks>
    /// Deserializing a §5.2 body into <see cref="TokenResponse"/> yields empty members
    /// with no way to tell "server refused" from "empty response". Branch on the HTTP
    /// status (a conformant server answers 400, §5.2) and bind the body here; some
    /// deployments answer 200, so a non-null <see cref="Error"/> is the robust
    /// shape-based test.
    /// </remarks>
    public class TokenErrorResponse
    {
        /// <summary>
        /// REQUIRED (§5.2): a single ASCII error code from the registry —
        /// <c>invalid_request</c>, <c>invalid_client</c>, <c>invalid_grant</c>,
        /// <c>unauthorized_client</c>, <c>unsupported_grant_type</c>, <c>invalid_scope</c>.
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// OPTIONAL (§5.2): human-readable explanation of what went wrong, for the
        /// client DEVELOPER only — MUST NOT be shown to the end user.
        /// </summary>
        [JsonPropertyName("error_description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorDescription { get; set; }

        /// <summary>
        /// OPTIONAL (§5.2): URI of a human-readable web page with information about
        /// the error.
        /// </summary>
        [JsonPropertyName("error_uri")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorUri { get; set; }
    }
}
