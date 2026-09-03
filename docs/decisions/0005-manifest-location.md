# 0005 — The provisioning manifest lives in the product's repository

## Decision

Each product keeps its Identity provisioning manifest in its own repository,
at `deploy/identity/identity-manifest.v1.json`, and the product's tests own
its invariants. Identity validates the schema and applies the plan at
provisioning time; it does not store or centralize the manifest.

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

## What would make this wrong

- **If a manifest change needed an atomic, cross-product commit** — access
  introduced in one product and consumed by another in the same change — the
  split would turn one decision into two pull requests that can drift.
  Centralize that pair, not the model.
- **If products repeatedly got the location or shape wrong**, the schema
  validation at `preview` is firing at the wrong layer; move the check into a
  shared test fixture instead of a convention the reviewer enforces.

The `deploy/identity/` directory holds what Identity consumes from the
product and nothing the product's own runtime reads. Current examples:
`sufficit-ai`, `sufficit-network-control`.
