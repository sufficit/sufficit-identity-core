#!/usr/bin/env python3
"""Generates docs/entitlements/ from the type declarations in src/.

The catalogue is public documentation, so it must never claim to be complete
while quietly dropping an entitlement whose declaration uses a shape this
script does not recognise. Anything unresolved is a hard failure, not a
warning: a catalogue that lies is worse than no catalogue.

Run from the repository root:

    python3 tools/generate-entitlement-catalogue.py
"""
from __future__ import annotations

import re
import sys
import pathlib

ROOT = pathlib.Path(__file__).resolve().parent.parent
SRC = ROOT / "src"
OUT = ROOT / "docs" / "entitlements"

# Folder name in src/ -> published feature name.
FEATURES = {
    "AI": "AI",
    "Cloud": "Cloud",
    "Exchange": "Exchange",
    "Finance": "Finance",
    "Gateway": "Gateway",
    "Identity": "Identity",
    "Provisioning": "Provisioning",
    "Relacionamento": "Relationship",
    "Sales": "Sales",
    "Telephony": "Telephony",
}

# Base abstractions, not entitlements.
ABSTRACTIONS = {"Entitlement", "IEntitlement", "ISelfContextEntitlement", "EntitlementBase"}


def literal(text: str, pattern: str) -> str | None:
    match = re.search(pattern, text)
    return match.group(1).strip() if match else None


def resolve_constant(name: str, text: str) -> str | None:
    """Resolves `X = "literal"`, following one `Type.Member` hop if needed."""
    direct = literal(text, rf'\b{re.escape(name)}\s*=\s*"([^"]+)"')
    if direct:
        return direct

    hop = literal(text, rf'\b{re.escape(name)}\s*=\s*([\w.]+)\s*;')
    if hop and "." in hop:
        owner, member = hop.rsplit(".", 1)
        for candidate in SRC.rglob(f"{owner}.cs"):
            return literal(
                candidate.read_text(encoding="utf-8-sig"),
                rf'\b{re.escape(member)}\s*=\s*"([^"]+)"')
    return None


def entitlement_key(text: str) -> str | None:
    # Key => "literal"   |   Key { get; } = "literal"
    direct = literal(text, r'string Key\s*(?:=>|\{\s*get;\s*\}\s*=)\s*"([^"]+)"')
    if direct:
        return direct

    # Key => SomeConstant;  (possibly pointing at another type's member)
    via = literal(text, r'string Key\s*(?:=>|\{\s*get;\s*\}\s*=)\s*([\w.]+)\s*;')
    if via:
        return resolve_constant(via.split(".")[-1], text) or resolve_constant(via, text)
    return None


def entitlement_name(text: str) -> str | None:
    return literal(text, r'string Name\s*(?:=>|\{\s*get;\s*\}\s*=)\s*"([^"]+)"')


def main() -> int:
    OUT.mkdir(parents=True, exist_ok=True)
    index: list[tuple[str, str, int]] = []
    unresolved: list[str] = []

    for folder, title in FEATURES.items():
        rows = []
        for path in sorted((SRC / folder).glob("*Entitlement.cs")):
            if path.stem in ABSTRACTIONS:
                continue

            text = path.read_text(encoding="utf-8-sig")
            key = entitlement_key(text)
            identifier = literal(text, r'UniqueID\s*=\s*"([^"]+)"')

            if not key or not identifier:
                unresolved.append(str(path.relative_to(ROOT)))
                continue

            rows.append((
                key,
                path.stem,
                entitlement_name(text) or "",
                identifier,
                "ISelfContextEntitlement" in text,
            ))

        if not rows:
            continue

        lines = [
            f"# {title} entitlements",
            "",
            f"{len(rows)} entitlement(s). The identifiers below are published constants of",
            "this library, not operational data.",
            "",
            "| Key | Type | Name | ID | Self-context |",
            "| --- | --- | --- | --- | --- |",
        ]
        for key, stem, name, identifier, self_context in rows:
            lines.append(
                f"| `{key}` | `{stem}` | {name} | `{identifier}` | "
                f"{'yes' if self_context else '—'} |")
        lines += [
            "",
            "A *self-context* entitlement resolves an empty stored context to the",
            "principal's own identifier — read the empty value as *their own*, never as",
            "*any*.",
            "",
            "See [Entitlements](../entitlements.md) for the value format and comparison",
            "rules.",
            "",
        ]
        (OUT / f"{title.lower()}.md").write_text("\n".join(lines), encoding="utf-8")
        index.append((title, title.lower(), len(rows)))

    if unresolved:
        print("Could not resolve Key or UniqueID for:", file=sys.stderr)
        for item in unresolved:
            print(f"  {item}", file=sys.stderr)
        print(
            "\nFix the declaration or teach this generator the shape. Publishing an\n"
            "incomplete catalogue as if it were complete is the failure being avoided.",
            file=sys.stderr)
        return 1

    if not index:
        print(
            "No entitlements found at all. Either the declarations moved or the\n"
            "file pattern is stale — an empty catalogue that exits successfully is\n"
            "the same silent lie this generator exists to prevent.",
            file=sys.stderr)
        return 1

    total = sum(count for _, _, count in index)
    readme = [
        "# Entitlement catalogue",
        "",
        f"{total} entitlements across {len(index)} feature areas. Each product owns its own",
        "vocabulary: an entitlement means nothing outside the feature that defines it,",
        "which is why they are grouped this way instead of flattened into one hierarchy.",
        "",
        "| Feature | Entitlements |",
        "| --- | --- |",
    ]
    for title, slug, count in index:
        readme.append(f"| [{title}]({slug}.md) | {count} |")
    readme += [
        "",
        "## Adding one",
        "",
        "A new entitlement needs a fresh `UniqueID`, a `Key` unique within its feature,",
        "and a `Name` a human can read. Never reuse an identifier: it is the equality",
        "key, so reuse silently grants the old permission to the new thing.",
        "",
        "## Regenerating",
        "",
        "This catalogue is generated from the type declarations:",
        "",
        "```sh",
        "python3 tools/generate-entitlement-catalogue.py",
        "```",
        "",
        "The generator fails when it cannot resolve an entitlement rather than omitting",
        "it, so a green run means the list is complete.",
        "",
    ]
    (OUT / "README.md").write_text("\n".join(readme), encoding="utf-8")

    print(f"{total} entitlements across {len(index)} features")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
