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
    }
}
