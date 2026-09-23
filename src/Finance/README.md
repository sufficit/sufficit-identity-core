# Finance — regras e diretivas financeiras

Este diretório define o **vocabulário canônico de autorização financeira** do
ecossistema Sufficit: 2 roles (papéis) e 12 entitlements (diretivas) que
governam saldo, pagamentos, despesas, boletos e notas fiscais.

Toda regra aqui é uma classe com **GUID imutável** e **key literal** — o valor
persistido nos claims dos usuários. Mudar qualquer um dos dois é uma migração
de dados, não um refactor.

---

## 1. Roles

| Role | `NormalizedName` | Filtros | UniqueID |
|---|---|---|---|
| `FinancialRole` | `financial` | `financial`, `financeiro` | `547c85dc15fc4730937798e1bacd0899` |
| `FinancialManagerRole` | `financialmanager` | `financialmanager`, `gerente financeiro` | `1ef82b4a532e4225b3db7619c5cd8870` |

Roles concedem acesso **por tenant inteiro** (via `[Authorize(Roles = ...)]`).
Entitlements concedem acesso **por contexto** (`key:contextId` no claim) — ver
seção 4.

## 2. Entitlements (inventário completo)

| Entitlement | Key | Nome (exibição) | Role vinculada | UniqueID |
|---|---|---|---|---|
| `FinanceEntitlement` | `finance` | acesso ao financeiro | ⚠️ **nenhuma** (`Guid.Empty`) | `20b48f3bd8394007bde438151740c6a9` |
| `BalanceViewEntitlement` | `balanceview` | visualizar saldo | `FinancialRole` | `1c8a1f496f3e49798885d52fc79f0dee` |
| `BalanceUpdateEntitlement` | `balanceupdate` | realizar movimentação financeira | `FinancialRole` | `cf50a644934748d9929618973a252a55` |
| `BalanceTransferEntitlement` | `balancetransfer` | transferir saldo | `FinancialRole` | `17f20ed119374f75b41931987a892ca0` |
| `PaymentEntitlement` | `payment` | efetuar pagamentos | `FinancialRole` ⚠️ *GUID hardcoded* | `51a8108b4a1f4a53abed12dde40b238d` |
| `ExpenseViewEntitlement` | `expenseview` | visualizar despesas | `FinancialRole` | `606a66368577417aa027d6f4a8ed00ea` |
| `ExpenseUpdateEntitlement` | `expenseupdate` | atualizar despesas | `FinancialRole` | `245a76686916494692a0edc702b27137` |
| `BankSlipEntitlement` | `bankbillet` ⚠️ *typo histórico* | acesso a boletos | `FinancialRole` | `1cea282f5b3645d685e6d1ad866d2b27` |
| `BankSlipManageEntitlement` | `bankslipmanage` | gerenciar boletos | `FinancialManagerRole` | `139e7b816b3c450a909ccd2b7b25f543` |
| `BankSlipPayerDataEntitlement` | `bankslippayerdata` | acessar dados do pagador de boletos | `FinancialManagerRole` | `9c1cc91856d1420cb6c035be272e768b` |
| `BankSlipRetentionEntitlement` | `bankslipretention` | gerenciar retenção de boletos | `FinancialManagerRole` | `7fb582fa089242c1baa20d53e20b29df` |
| `BankSlipSettingsEntitlement` | `bankslipsettings` | configurar boletos | `FinancialManagerRole` | `0416f398e9df41909f704ca015ee5806` |

### Hierarquia de herança

```
Entitlement (base, src/Identity)
├── FinanceEntitlement                (sem role)
├── BalanceView / BalanceUpdate / BalanceTransfer   → FinancialRole
├── Payment / ExpenseView / ExpenseUpdate           → FinancialRole
└── BankSlipEntitlement (bankbillet)  → FinancialRole
    └── BankSlipManageEntitlement     → FinancialManagerRole (new UniqueID/RoleID/Key)
        ├── BankSlipPayerDataEntitlement
        ├── BankSlipRetentionEntitlement
        └── BankSlipSettingsEntitlement
```

> A herança em `BankSlip*` é **semântica** (gerenciar ⇒ pressupõe acesso), mas
> cada nível redeclara `UniqueID`, `RoleID` e `Key` com `new const` — cada
> entitlement é uma permissão independente no banco e nos claims.

## 3. Distribuição — quem consome cada regra

Varredura completa do ecossistema (`sufficit-*` + `hemocardio-coop`, código
C#). **"—"** = sem consumidor encontrado no código.

| Key | sufficit-endpoints (API) | sufficit-blazor (UI) | sufficit-web (legado aspx) | sufficit-standard (legado core) | sufficit-client |
|---|---|---|---|---|---|
| `finance` | `ElectronicInvoiceController` — `ThrowIfUnauthorized` (NF-e) | — | — | — | — |
| `balanceview` | `FinanceController` — histórico/saldo | `FinanceHistoryPage` — `ContextView.Default<T>` | `Historico.aspx` — `Directive` | — | — |
| `balanceupdate` | — | — | — | `SCFinanceiro` — `HasEntitlement<T>()` | — |
| `balancetransfer` | ⚠️ Transfers usa **Roles** (Manager/Admin) | ⚠️ `TransfersPage` usa **Roles** (Manager/Admin) | `Transferencias.aspx`, `Controle.aspx` — `HasEntitlement<T>()` | — | — |
| `payment` | — | catálogo de acesso guiado | `Pagamento.aspx` — `Directive` | `Wizard.cs` — `EnsureUserPolicy<T>` | — |
| `expenseview` | **sem consumidor** | **sem consumidor** | **sem consumidor** | **sem consumidor** | **sem consumidor** |
| `expenseupdate` | — | — | `ContasAPagar.aspx` — `HasEntitlement<T>(context)` | — | — |
| `bankbillet` | `BankSlipController` — view/PDF | `BankSlipDashboardPage` — `Default<T>`; `MyBankSlipsPage` usa **Roles** | — | menção em log (`SCFinanceiro`) | `LegacyBankSlipControllerSection` usa **Roles** |
| `bankslipmanage` | `BankSlipController` — emitir/cancelar (`RequireEntitlement<T>`) | — | — | — | — |
| `bankslipsettings` | `BankSlipController` (settings) + `BankSlipProviderDiagnosticsController` | `BankSlipDashboardPage` — `HasPolicy<T>` | — | — | — |
| `bankslippayerdata` | **sem consumidor** | **sem consumidor** | **sem consumidor** | **sem consumidor** | **sem consumidor** |
| `bankslipretention` | **sem consumidor** | **sem consumidor** | **sem consumidor** | **sem consumidor** | **sem consumidor** |

### Onde as roles aparecem (`[Authorize(Roles = ...)]`)

| Role | Consumidores |
|---|---|
| `financialmanager` | `FinanceController.RecentPayments` (API), `CreditsController` (API), `RecentPaymentsPage` (blazor), `MyBankSlipsPage` (blazor), `LegacyBankSlipControllerSection` (client) — sempre junto de `manager`/`administrator` |
| `financial` | `MyBankSlipsPage` (blazor), `LegacyBankSlipControllerSection` (client) |

`sufficit-blazor-ai-access` é um worktree do `sufficit-blazor` (mesmo código) —
não conta como consumidor adicional.

## 4. Como uma diretiva chega ao usuário (contrato)

1. **Claim**: o token carrega `<key>:<contextId>` no claim `entitlements`
   (RFC 9068, `ClaimTypes.Entitlement`) e, durante a transição, também no nome
   histórico `directive` (`ClaimTypes.Directive`). Leitores aceitam os dois —
   ver `docs/decisions/0001-entitlement-naming.md`.
2. **Classes são descobertas por reflexão**: `Entitlement.Enumerator` varre todo
   `IEntitlement` com `Key` não-vazio. Um arquivo novo neste diretório entra no
   catálogo automaticamente.
3. **Conformance compartilhado**: `EntitlementConformance` (em `src/Identity`)
   define casos que todo app deve responder igual — contexto compacto sem
   hífen resolve igual, grant de outro contexto não vale, valor com espaço não
   concede nada, key desconhecida não concede nada etc. Executado nos testes
   de cada consumidor.
4. **Duas idiomáticas de guarda** (coexistem hoje):
   - **Entitlement por contexto** — `ThrowIfUnauthorized<T>(contextId)`,
     `RequireEntitlement<T>(contextId)`, `ContextView.Default<T>()`,
     `HasPolicy<T>(contextId)`, e no legado `HasEntitlement<T>()` /
     `Directive` / `EnsureUserPolicy<T>`.
   - **Role por tenant** — `[Authorize(Roles = FinancialManagerRole.NormalizedName, ...)]`
     (RecentPayments, Transfers, Credits, MyBankSlips).

## 5. Convenções e divergências conhecidas

| # | Item | Situação |
|---|---|---|
| 1 | `BankSlipEntitlement.Key = "bankbillet"` | Typo histórico (deveria ser `bankslip`), documentado no próprio código como *"wrong key name in code, change in future"*. Permanece porque o valor persiste em claims e no catálogo de acesso guiado. Qualquer renomeação exige migração de dados. |
| 2 | `PaymentEntitlement.RoleID` | GUID hardcoded como string (`"547c85dc-15fc-..."`) em vez de `FinancialRole.UniqueID`. Valor idêntico, grafia diferente. Convenção do repo (guardada por `EntitlementKeyDecouplingTests` para telephony): referenciar a constante da role, nunca duplicar o literal. |
| 3 | `FinanceEntitlement` sem `IDRole` | Único do diretório sem role vinculada (`Guid.Empty`). Não herdou o padrão `RoleID = FinancialRole.UniqueID`. |
| 4 | Transfers: entitlement × role | O legado (`sufficit-web`) guarda transferências por `BalanceTransferEntitlement`; a stack nova (API `FinanceController.Transfers` e `TransfersPage`) guarda por **Roles** manager/administrator. O entitlement segue válido no legado, mas a stack nova não o consome. |
| 5 | Diretivas sem nenhum consumidor | `expenseview`, `bankslippayerdata`, `bankslipretention` existem no vocabulário (e aparecem na reflexão/claims) mas nenhuma tela/endpoint atual as verifica. Definidas para uso futuro ou herdadas de fluxos desativados. |
| 6 | Usings mortos | Vários arquivos carregam `System.Collections.Generic`/`System.Text` sem uso (cosmético, herdado do template). |
| 7 | Acesso guiado | `GuidedAccessCatalog` (blazor) expõe `payment`, `balanceview`, `bankbillet` com descrição em PT para o admin conceder; `GuidedPoliciesTests` cobre essas keys. |

## 6. Checklist para adicionar uma nova diretiva financeira

1. Um arquivo por classe, nome `<Recurso><Ação>Entitlement.cs`, namespace
   `Sufficit.Finance`.
2. `UniqueID` = GUID novo e definitivo (gerar uma vez; nunca reutilizar).
3. `Key` = literal minúsculo, sem referenciar constante de role
   (`EntitlementKeyDecouplingTests`).
4. `RoleID` = constante da role (`FinancialRole.UniqueID` /
   `FinancialManagerRole.UniqueID`) — herdando da classe irmã quando for
   sub-permissão (padrão `BankSlip*`).
5. `Name` em português, verbos no infinitivo ("visualizar…", "gerenciar…").
6. A classe entra sozinha no `Entitlement.Enumerator` (reflexão por `Key`
   não-vazio) — confirme com um teste que a enumera.
7. Consumidor precisa de guarda: preferir verificação por contexto
   (`ThrowIfUnauthorized<T>`/`RequireEntitlement<T>` na API,
   `ContextView.Default<T>`/`HasPolicy<T>` no blazor); usar
   `[Authorize(Roles…)]` só quando a semântica for realmente por tenant.
