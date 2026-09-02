using System.Text.RegularExpressions;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

/// <summary>
/// The scope naming rules from
/// <c>docs/decisions/0004-scope-naming-and-audience.md</c>, enforced against
/// the document that publishes them.
/// </summary>
/// <remarks>
/// A convention stated only in prose is followed until the first hurry. The
/// names that broke the previous convention were not added by someone who
/// disagreed with it — they were added by someone who had not read it.
/// </remarks>
public sealed class ScopeNamingTests
{
    /// <summary>Bare names are reserved for the claim a scope releases.</summary>
    private static readonly string[] ClaimReleasing =
    [
        "openid", "profile", "email", "address", "offline_access",
        "roles", "entitlements",
    ];

    private static string Document()
    {
        var path = Path.Combine(RepositoryRoot(), "docs", "scopes.md");
        Assert.True(File.Exists(path), $"missing: {path}");
        return File.ReadAllText(path);
    }

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !Directory.Exists(Path.Combine(directory.FullName, "docs")))
            directory = directory.Parent;

        Assert.NotNull(directory);
        return directory!.FullName;
    }

    /// <summary>
    /// Claim names appear next to the scope that releases them. They are not
    /// scopes and must not be held to the scope naming rules.
    /// </summary>
    private static readonly string[] ClaimNames = ["role"];

    /// <summary>
    /// Every scope named in the document's tables.
    /// </summary>
    /// <remarks>
    /// Table rows only. Prose quotes the forms being rejected — reading those
    /// as scopes would make the guard fail on the very text that defines it.
    /// </remarks>
    private static string[] DocumentedScopes() =>
        Document()
            .Split('\n')
            .Where(line => line.TrimStart().StartsWith("|", StringComparison.Ordinal))
            .SelectMany(line => Regex.Matches(line, @"`([a-z][a-z0-9_.]*)`")
                .Select(m => m.Groups[1].Value))
            .Where(value => !ClaimNames.Contains(value, StringComparer.Ordinal))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

    [Fact]
    public void No_scope_carries_the_redundant_product_prefix()
    {
        var offenders = DocumentedScopes()
            .Where(scope => scope.StartsWith("sufficit_", StringComparison.Ordinal)
                || scope.StartsWith("sufficit.", StringComparison.Ordinal))
            .ToArray();

        Assert.True(offenders.Length == 0,
            "every scope here is Sufficit's; the prefix carries no information: "
                + string.Join(", ", offenders));
    }

    [Fact]
    public void An_api_scope_is_dotted_lowercase_and_never_snake_case()
    {
        var offenders = DocumentedScopes()
            .Where(scope => !ClaimReleasing.Contains(scope, StringComparer.Ordinal))
            .Where(scope => !Regex.IsMatch(scope, @"^[a-z][a-z0-9]*(\.[a-z][a-z0-9]*)+$"))
            .ToArray();

        Assert.True(offenders.Length == 0,
            "API scopes are <product>.<capability>: " + string.Join(", ", offenders));
    }

    /// <summary>
    /// The rule that actually protects something: a login scope must not be
    /// able to mint a token accepted by an unrelated API.
    /// </summary>
    [Fact]
    public void The_document_states_that_claim_releasing_scopes_declare_no_resource()
    {
        Assert.Contains("declares no resource", Document(), StringComparison.Ordinal);
    }
}
