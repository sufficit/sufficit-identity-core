using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Xunit;

namespace Sufficit.Identity.Core.Tests;

/// <summary>
/// The documentation in this repository is public. It is meant to describe how
/// the platform works, never what it runs.
/// </summary>
/// <remarks>
/// Documentation does not leak by decision — it leaks when someone pastes a
/// real example while in a hurry. A reminder in a contributing guide is exactly
/// the kind of rule that lives only in somebody's head; a test is the kind that
/// survives.
/// <para>
/// Publishing the design is deliberate: security rests on the secrecy of keys,
/// not of design. What must not appear is the inventory — host names,
/// addresses, ports, and identifiers of real tenants or accounts, which let a
/// stranger skip reconnaissance.
/// </para>
/// </remarks>
public sealed class DocumentationLeakTests
{
    /// <summary>
    /// Obviously-fake identifiers. Examples get copied, so they must be
    /// impossible to mistake for something real.
    /// </summary>
    private static readonly string[] ExampleIdentifiers =
    [
        "11111111-1111-1111-1111-111111111111",
        "11111111111111111111111111111111",
        "00000000-0000-0000-0000-000000000000",
    ];

    [Fact]
    public void Documents_do_not_name_internal_hosts_or_addresses()
    {
        var offenders = new List<string>();

        foreach (var file in Documents())
        {
            var text = File.ReadAllText(file);

            // Private ranges (RFC 1918) and loopback: an address in a document
            // is a map of the estate, never an explanation of the design.
            Check(offenders, file, text,
                @"\b(?:10|127)\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", "private address");
            Check(offenders, file, text,
                @"\b192\.168\.\d{1,3}\.\d{1,3}\b", "private address");
            Check(offenders, file, text,
                @"\b172\.(?:1[6-9]|2\d|3[01])\.\d{1,3}\.\d{1,3}\b", "private address");

            // Internal host naming and shell prompts pasted from a session.
            Check(offenders, file, text, @"\b[\w.-]+\.local\b", "internal host");
            Check(offenders, file, text, @"root@[\w.-]+", "operator account");
            Check(offenders, file, text, @"\b/etc/sufficit\b", "server path");
        }

        Assert.True(offenders.Count == 0, string.Join("\n", offenders));
    }

    [Fact]
    public void Identifiers_in_documents_are_either_examples_or_published_constants()
    {
        // A GUID is acceptable when it is a constant of this library — those are
        // public by definition. Any other one is suspicious: it may be a real
        // tenant, user or account, and those identify customers.
        var published = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var source in Directory.EnumerateFiles(
            Path.Combine(RepositoryRoot(), "src"), "*.cs", SearchOption.AllDirectories))
        {
            foreach (Match match in Guids().Matches(File.ReadAllText(source)))
            {
                published.Add(Canonical(match.Value));
            }
        }

        var offenders = new List<string>();
        foreach (var file in Documents())
        {
            foreach (Match match in Guids().Matches(File.ReadAllText(file)))
            {
                var value = Canonical(match.Value);
                if (published.Contains(value)
                    || ExampleIdentifiers.Select(Canonical)
                        .Contains(value, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                offenders.Add(
                    $"{Path.GetFileName(file)}: identifier {match.Value} is neither an "
                    + "example nor a published constant — if it is real, it names a "
                    + "customer.");
            }
        }

        Assert.True(offenders.Count == 0, string.Join("\n", offenders));
    }

    private static void Check(
        List<string> offenders, string file, string text, string pattern, string what)
    {
        foreach (Match match in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
        {
            offenders.Add($"{Path.GetFileName(file)}: {what} — {match.Value}");
        }
    }

    /// <summary>
    ///     Matches both spellings of an identifier.
    /// </summary>
    /// <remarks>
    ///     Recognising only the hyphenated form would let the compact one through
    ///     unchecked — the same value, a different spelling, and the guard blind to
    ///     half of it. That is the exact hazard the entitlement documentation warns
    ///     about, and it applies here too.
    /// </remarks>
    private static Regex Guids() => new(
        @"\b(?:[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}"
        + @"|[0-9a-fA-F]{32})\b",
        RegexOptions.Compiled);

    /// <summary>Both spellings normalise to the same key before comparison.</summary>
    private static string Canonical(string identifier) =>
        Guid.TryParse(identifier, out var parsed) ? parsed.ToString("N") : identifier;

    private static IEnumerable<string> Documents()
    {
        var root = RepositoryRoot();
        yield return Path.Combine(root, "README.md");

        var docs = Path.Combine(root, "docs");
        if (!Directory.Exists(docs)) yield break;

        foreach (var file in Directory.EnumerateFiles(
            docs, "*.md", SearchOption.AllDirectories))
        {
            yield return file;
        }
    }

    private static string RepositoryRoot([CallerFilePath] string sourceFile = "") =>
        Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(sourceFile)
                ?? throw new InvalidOperationException(
                    "Unable to resolve the test source directory."),
            "..", ".."));
}
