using System.Text.Json;
using Sufficit.Identity;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

/// <summary>
/// RFC 6749 conformance of the client-side token response model.
/// </summary>
/// <remarks>
/// Section 5.1 makes <c>refresh_token</c> OPTIONAL and <c>scope</c> OPTIONAL
/// when identical to the requested scope. Section 4.4.3 goes further: the
/// client-credentials grant SHOULD NOT be issued a refresh token at all. A
/// DTO whose properties are non-nullable turns every legal response of that
/// shape into a null-reference exception one line later, in the consumer.
/// </remarks>
public sealed class TokenResponseOptionalityTests
{
    private static readonly JsonSerializerOptions Wire =
        new(JsonSerializerDefaults.Web);

    [Fact]
    public void A_client_credentials_response_without_refresh_token_or_scope_is_legal()
    {
        // Exactly the example of RFC 6749 §4.4.3.
        var json = """
        {
          "access_token": "2YotnFZFEjr1zCsicMWpAA",
          "token_type": "example",
          "expires_in": 3600
        }
        """;

        var response = JsonSerializer.Deserialize<TokenResponse>(json, Wire)!;

        Assert.Equal("2YotnFZFEjr1zCsicMWpAA", response.AccessToken);
        Assert.Equal("example", response.TokenType);
        Assert.Equal(3600, response.ExpiresIn);

        // OPTIONAL members are absent: reading them yields null, never a
        // null-reference exception hidden behind a non-nullable property.
        Assert.Null(response.RefreshToken);
        Assert.Null(response.Scope);

        // §5.1 omits OPTIONAL members rather than emitting them as null: a
        // server re-serializing this DTO must not put them on the wire either.
        var reserialized = JsonSerializer.Serialize(response, Wire);
        Assert.DoesNotContain("refresh_token", reserialized);
        Assert.DoesNotContain("scope", reserialized);
        Assert.DoesNotContain("id_token", reserialized);
    }

    [Fact]
    public void A_full_response_still_binds_every_member()
    {
        // The RFC 6749 §5.1 example, with the OPTIONAL members present.
        var json = """
        {
          "access_token": "2YotnFZFEjr1zCsicMWpAA",
          "token_type": "Bearer",
          "expires_in": 3600,
          "refresh_token": "tGzv3JOkF0XG5Qx2TlKWIA",
          "scope": "read write"
        }
        """;

        var response = JsonSerializer.Deserialize<TokenResponse>(json, Wire)!;

        Assert.Equal("tGzv3JOkF0XG5Qx2TlKWIA", response.RefreshToken);
        Assert.Equal("read write", response.Scope);
    }

    [Fact]
    public void The_introspection_request_accepts_the_optional_hint()
    {
        // RFC 7662 §2.1: token REQUIRED, token_type_hint OPTIONAL. Both spell
        // out on the wire exactly as registered.
        var withHint = JsonSerializer.Serialize(
            new TokenValidationRequest { Token = "t", TokenTypeHint = "access_token" }, Wire);
        var withoutHint = JsonSerializer.Serialize(
            new TokenValidationRequest { Token = "t" }, Wire);

        Assert.Contains("\"token_type_hint\":\"access_token\"", withHint);
        Assert.DoesNotContain("token_type_hint", withoutHint);
    }

    [Fact]
    public void An_omitted_expires_in_yields_null_not_zero()
    {
        var json = """
        {
          "access_token": "t",
          "token_type": "Bearer"
        }
        """;

        var response = JsonSerializer.Deserialize<TokenResponse>(json, Wire)!;

        // §5.1: expires_in is RECOMMENDED but omittable. Absence is null — it
        // must not collapse into 0, an "expires immediately" sentinel.
        Assert.Null(response.ExpiresIn);
        Assert.DoesNotContain("expires_in", JsonSerializer.Serialize(response, Wire));
    }

    [Fact]
    public void The_section_5_2_error_shape_binds_to_its_own_dto()
    {
        var json = """
        {
          "error": "invalid_grant",
          "error_description": "code was already redeemed",
          "error_uri": "https://example.com/docs#invalid-grant"
        }
        """;

        var error = JsonSerializer.Deserialize<TokenErrorResponse>(json, Wire)!;

        Assert.Equal("invalid_grant", error.Error);
        Assert.Equal("code was already redeemed", error.ErrorDescription);
        Assert.Equal("https://example.com/docs#invalid-grant", error.ErrorUri);

        // Round-trip: OPTIONAL members stay off the wire when null (§5.2).
        var minimal = JsonSerializer.Deserialize<TokenErrorResponse>(
            """{"error":"invalid_client"}""", Wire)!;
        var serialized = JsonSerializer.Serialize(minimal, Wire);
        Assert.Contains("\"error\":\"invalid_client\"", serialized);
        Assert.DoesNotContain("error_description", serialized);
        Assert.DoesNotContain("error_uri", serialized);
    }
}
