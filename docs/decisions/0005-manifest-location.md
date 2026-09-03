# 0005 — The provisioning manifest lives in the product's repository

## Decision

Each product keeps its Identity provisioning manifest in its own repository,
at the **repository root**: `identity-manifest.v1.json`. The product's tests
own its invariants. Identity validates the schema and applies the plan at
provisioning time; it does not store or centralize the manifest.

The location is the root — not a nested directory — so every sufficit product
looks the same from the outside: one canonical file, one path, no
per-product discovery. Current examples: `sufficit-ai`,
`sufficit-network-control`, `sufficit-phone`.

## What was rejected

**A central registry of manifests.** One place holding every product's
manifest couples Identity's review flow to every product's scope change and
moves a product's access declaration away from the code and configuration it
describes. A scope request lands better as a pull request where the product's
own tests run, not as an edit in a repository the product team does not work
in.

**Generating the manifest from application code.** The manifest is metadata
about authorization, not a build artifact: it changes when access changes,
which is not the same moment the application builds. Deriving it from code
would make a scope appear and disappear with release branches.

**A nested convention (`deploy/identity/`).** This decision's original
location, amended to the root. It buried a top-level contract inside a
product-specific directory tree, and products without a `deploy/` layout
(`sufficit-phone`) would have to either invent a new nesting or break the
convention on adoption.

## What would make this wrong

- **If a manifest change needed an atomic, cross-product commit** — access
  introduced in one product and consumed by another in the same change — the
  split would turn one decision into two pull requests that can drift.
  Centralize that pair, not the model.
- **If products repeatedly got the location or shape wrong**, the schema
  validation at `preview` is firing at the wrong layer; move the check into a
  shared test fixture instead of a convention the reviewer enforces.
