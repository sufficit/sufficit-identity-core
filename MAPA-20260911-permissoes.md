# Mapa do Sistema de Permissões — o que temos hoje e o que precisaríamos mudar

Data: 2026-09-11 (rev. 12/09) · Fonte: varredura de código (6 repositórios) + banco de produção (`identity` em eveo-proxy-local) ·
Companheiros: [PLAN-20260911-admin-suffix-migration.md](PLAN-20260911-admin-suffix-migration.md) (fases da migração de nomes) ·
[ESTUDO-20260912](../../sufficit-endpoints/docs/security/ESTUDO-20260912-endpoints-permissoes-read-write.md)
(superfície do sufficit-endpoints: 602 rotas auditadas, evidência de produção e proposta `.read`/`.write`)

---

## 1. Como o sistema funciona hoje

```
CONCESSÃO                     EMISSÃO                        ENFORCEMENT
Wizard (onboarding)    ──┐    userclaims (claim             ┌─ EndPoints: [EntitlementRequirement] ·
API /api/claims        ──┼──▶ 'entitlements'/'directive') ──┼─   User.ThrowIfUnauthorized<T>()
Escopos c/ entitlement ──┘    + roles (de userroles)        ├─ Blazor: ContextView.Default<T> ·
(scopes.properties)           gated por ClaimScopeMap       │   AuthorizeView Roles="…"
                              (entitlements→scope           ├─ Legado WebForms: HasEntitlement<T> ·
                              'directives')                 │   IsInRole<T> (UserPrincipal)
                                                            └─ API Identity: capacidades (RoleCapabilities)
```

**Dois modelos de resolução de papel coexistem — e divergem:**

| | Legado WebForms (sufficit-web) | Blazor + EndPoints (OIDC) |
|---|---|---|
| Principal | `UserPrincipal` construído no servidor (`SCUserSession`/`Secao`) | Token JWT/cookie do STS |
| Roles | **derivadas dos entitlements** (`Utils.GetRoles`) — `telephonyadmin` etc. funcionam | **só de `userroles` no banco** — hoje apenas `administrator`, `manager`, `mobilecloudadministrator` |
| Consequência | gates por role-string funcionam | **qualquer role-string de papel derivado nunca passa** (ver §3.2) |

Armazenamento (banco `identity`, schema custom): `userclaims`, `roles`, `userroles`, `roleclaims` (vazia),
`applications.properties` (`identity:client:roles` para machine clients), `scopes.properties`
(`identity:scope:entitlement-claims`). Backups de migrações anteriores:
`userclaims_bkp_entitlement_20260902`, `userclaims_bkp_directive_20260909`, `userroles_bkp_admin_20260902`.

Concessores automáticos: Wizard concede `audioupdate`, `phonecalls`, `telephonyclient`, `payment` ao
membro no onboarding (`sufficit-standard/src/Telefonia/Wizard.cs:123-126`); escopos `ai.bridge` e
`sufficit_ai_openai_bridge` concedem `aiuser:000…0` a quem aprova.

---

## 2. Tudo o que temos hoje

### 2.1 Entitlements — 34 (chave, usuários em produção, onde é exigido)

Varridos: sufficit-endpoints, sufficit-blazor, sufficit-web, sufficit-standard, sufficit-identity, sufficit-efdata.
“— sem enforcement” = não encontrado nesses 6; **falta varrer** sufficit-ai, sufficit-cloud-mobile (Kotlin), asterisk-core (§3.4).

| Chave | Classe (`…Entitlement`) | UID | Usuários | Enforcement (onde) |
|---|---|---|---|---|
| `telephonyclient` | TelephonyClient | `825e32b4…26f7` | **1252** | o mais usado: ContextView×23, ThrowIfUnauthorized×6, EntitlementRequirement×2, HasEntitlement×5 (EndPoints, IVR, Outbound, Trunk, VoiceMail, Zabbix, Hermes…) |
| `phonecalls` | PhoneCalls | `cf3c66ab…86de` | **1238** | EntitlementRequirement×4, ThrowIfUnauthorized×4, HasEntitlement×4 (Call, EventsPanel, Monitor, IVR, MCP) |
| `payment` | Payment | `51a8108b…238d` | **1139** | concessão Wizard; enforcement fora dos varridos (mobile) |
| `audioupdate` | AudioUpdate | `b327fb38…2d5` | **1034** | concessão Wizard + legado `Audio.aspx:56` (acessos) |
| `audioadmin` ⚠️ | AudioAdmin | `d05f3e2b…4e6f` | 119 | legado `Audio.aspx:44` (permissão de incluir) |
| `bankbillet` | BankSlip | `1cea282f…2b27` | 46 | ThrowIfUnauthorized LegacyBankSlip + ContextView BankSlipDashboard |
| `dialplanupdate` | DialPlanUpdate | `2c248f08…486d` | 57 | o mais espalhado: ThrowIfUnauthorized×5 (WhatsApp, CallDispatch, CallForward, IVR), HasEntitlement×8 (Zabbix, ACD), ContextView×2, EntitlementRequirement×1 |
| `monitorchannels` | MonitorChannels | `7bc67d43…0618` | 32 | — sem enforcement nos varridos (RoleID=TelephonySupervisorRole) |
| `telephonyadmin` ⚠️ | TelephonyAdmin | `09394ab4…4032` | 24 | ContextView×6 (DID Dashboard, IVR, ACD, Calls, PhoneSetup, Free), EntitlementRequirement (DID, EndPoint/Aliases), ThrowIfUnauthorized (DID/ByContext no branch do piloto) |
| `aiuser` | AIUser | `12aca70d…3260` | 30 (+5 `directive`) | — nos varridos; concedido por escopo `ai.bridge` (consumidor: sufficit-ai) |
| `balanceview` | BalanceView | `1c8a1f49…f0dee` | 20 | ThrowIfUnauthorized FinanceController + ContextView FinanceHistory |
| `serviceupdate` | ServiceUpdate | `3aa87d8a…7b37` | 12 | só referência (1) |
| `clientadmin` ⚠️ | ClientAdmin | `9d7c9980…f634` | 9 | legado `Clientes.aspx:13` |
| `policyupdate` | PolicyUpdate | `96910621…e6f4` | 7 | — nos varridos |
| `provisioningadmin` ⚠️ | ProvisioningAdmin | `2353d733…5063` | 6 | EntitlementRequirement×5 rotas (ProvisioningController: Device CRUD + Attributes) |
| `bankslipmanage` | BankSlipManage | `139e7b81…f543` | ? | só referências (2) |
| `bankslippayerdata` | BankSlipPayerData | `9c1cc918…e68b` | ? | — |
| `bankslipretention` | BankSlipRetention | `7fb582fa…b29df` | ? | — |
| `bankslipsettings` | BankSlipSettings | `0416f398…e5806` | ? | ThrowIfUnauthorized BankSlipProviderDiagnostics |
| `expenseupdate` | ExpenseUpdate | `245a7668…7137` | ? | legado `ContasAPagar.aspx` |
| `expenseview` | ExpenseView | `606a6636…00ea` | ? | — |
| `finance` | Finance | `20b48f3b…c6a9` | ? | ThrowIfUnauthorized ElectronicInvoiceController |
| `gatewaydiagnostics` | GatewayDiagnostics | `1dc24b3a…61c9` | ? | ThrowIfUnauthorized V2/Gateway + ContextView×4 (Asaas, Efi, ProviderLab) |
| `portability` | Portability | `fe921659…f90a` | ? | ThrowIfUnauthorized PortabilityController |
| `balanceupdate` | BalanceUpdate | `cf50a644…2a55` | 3 | HasEntitlement SCFinanceiro |
| `balancetransfer` | BalanceTransfer | `17f20ed1…ca0` | 3 | HasEntitlement×3 (SCVendas, Transferencias, Controle) |
| `servicemonitor` | ServiceMonitor | `362a9b8f…d2f5` | 3 | — |
| `customergroup` | CustomerGroup | `77263022…2f93` | 3 | só referência (1) |
| `aicontrol` | AIControl | `43c12678…be4d` | 3 | — |
| `mobile` | Mobile | `f0a8bba0…e3c1` | 2 | — nos varridos (consumidor: cloud-mobile) |
| `contactdeny` | ContactUpdateDeny | `9a989de2…9491` | ? | só referências (2) |
| `groupcontacts` | GroupContacts | `26b32b5b…c1f1d` | ? | só referência (1) |
| `exchangemanager` | ExchangeManager | `2c45c5ac…c83b` | ? | ThrowIfUnauthorized Exchange/TemplateController |

⚠️ = carrega sufixo **admin** (objeto da migração).

### 2.2 Papéis — 13 no código + 1 só no banco

**No banco `roles`/`userroles` (o que entra no token):** `administrator` (3 usuários), `manager` (10),
`mobilecloudadministrator` (1; sem classe no código). `userroles` hoje tem 14 linhas (backup 02/09 tinha 6).

**Derivados por código (`IRole`, não vivem no banco):**

| Papel (NormalizedName) | Usado em | Obs. |
|---|---|---|
| `administrator` | IsInRole×5, role-string×129 (Blazor), FullAdministratorRoles, Role.Compare legado | **global — mantém** |
| `manager` | IsInRole×4, role-string×126 (Blazor), RoleCapabilities (4 capacidades identity.*) | global |
| `telephony` | role-string×42, IsInRole×1 | base da cadeia de telefonia |
| `telephonysupervisor` | IsInRole×2 (legado Ramais/VincularRamal), role-string×1 | pai de `monitorchannels` |
| `telephonymanager` | role-string×1 (TimeConditionController) | quase sem uso — candidato a receber a fusão |
| `telephonyadmin` ⚠️ | IsInRole×1, role-string×3 (BillingController, PortabilityController, Gateway SideBar) | **acoplado à chave do entitlement** |
| `salesmanager` | IsInRole×1, role-string×8 (Sales controllers, BillingController) | |
| `salesrepresentative` | — | **sem uso nenhum** |
| `financial` | role-string×4 | |
| `financialmanager` | role-string×13 (BankSlip, Credits, History) | |
| `exchangemanager` (role) | — | sem uso como papel (só como entitlement) |
| `provisioning` | role-string×1 (ProvisioningSelfService) | |
| `mobilecloudadministrator` ⚠️ | banco/config: RoleCapabilities → `identity.vault.secrets.*`; **5 machine clients** com `identity:client:roles` em `applications.properties`: `sufficit_cloud_mobile_api`, `sufficit-endpoints-vault`, `sufficit_ai_vault`, `sufficit-endpoints-vault-v3`, 1 fleet `credential-management` (`ec813c55…1700`) | papel de confiança do vault |

### 2.3 Escopos e claims

- Claim container nova: `entitlements`; histórica: `directive` (ambas aceitas na leitura; STS emite as duas em userinfo).
- `ClaimScopeMapOptions.ClaimToScope`: `directive→directives`, `entitlements→directives` (claim só entra no token com o escopo).
- Escopos com entitlement automático (`scopes.properties`): `ai.bridge` → `entitlements: aiuser:000…0`; `sufficit_ai_openai_bridge` → `directive: aiuser:000…0`.
- Escopos de produto: `chrome.phone`, `policies`, `roles`, `profile`… (clientes: extensão, mobile, swagger-ui).

### 2.4 Conformance e testes que travam o comportamento

- `EntitlementConformance.Cases` (identity-core): usa `audioadmin` como chave de teste (muda com o rename).
- `ClaimScopeMapTests`, `ManagementApplicationAuthorizationTests`, `ServiceAccountManagementTests` (sufficit-identity): referenciam `telephonyadmin`, `clientadmin`, `mobilecloudadministrator` por string.

---

## 3. Tudo o que precisaríamos mudar

### 3.1 Nomes com sufixo "admin" (o pedido original) → detalhes no PLAN-20260911

| Hoje | Proposto | Usuários afetados |
|---|---|---|
| `telephonyadmin` (entitlement) | **split**: `telephony.did.read`/`.write` + `telephony.endpoint.write` + `telephony.cost.read`/`.write` (alias antigo resolve como o conjunto) | 24 |
| `audioadmin` | `audio.write` (e `audioupdate`→`audio.update`) | 119 |
| `clientadmin` | `sales.client.read` + `sales.client.write` | 9 |
| `provisioningadmin` | `provisioning.read` + `provisioning.write` | 6 |
| papel `telephonyadmin` | fundir em `telephonymanager` | código |
| papel `mobilecloudadministrator` | `vaultsecrets` | 1 usuário + 5 machine clients + RoleCapabilities |
| `administrator` global | **mantém** | — |

### 3.2 Gates por role-string insatisfazíveis no token (bug latente, corrigir na migração)

Roles no token vêm **só** de `userroles` (`GrantOperations.cs:149`, `GetRolesAsync`), e o banco só tem
`administrator`/`manager`/`mobilecloudadministrator`. Logo, hoje, ninguém passa por gates que citam papéis
derivados — possivelmente quebrados desde a migração de papéis de 02/09 (backup `userroles_bkp_admin_20260902`):

| Gate | Arquivo | Papéis citados |
|---|---|---|
| `BalanceController.Patch` | endpoints `Telephony/BalanceController.cs` | `telephonyadmin,salesmanager` |
| `BillingController` Cost (POST/DELETE/GET) | endpoints `Telephony/BillingController.cs:40,54,66` | `telephonyadmin,salesmanager` |
| `TimeConditionController` | endpoints `Telephony/TimeConditionController.cs:105` | `telephonymanager` |
| `PortabilityController` | endpoints `Telephony/PortabilityController.cs:130` | `administrator,manager,telephonyadmin` |

No Blazor o efeito é de navegação: `AuthorizeView Roles=` com papéis derivados (Gateway SideBar, BankSlip
`financial*`, Exchange, AI Admin…) esconde seções para todos exceto `manager`/`administrator`.
**Correção:** converter os 4 gates de EndPoints para `EntitlementRequirement` (granular, por contexto — que é o
modelo correto) e auditar as strings do Blazor.

### 3.3 Acoplamento chave↔papel

`TelephonyAdminEntitlement.NormalizedKey = TelephonyAdminRole.NormalizedName` (mesma constante). Renomear o
papel mudaria a chave persistida silenciosamente. **Pré-requisito da fase 0** (✅ feito 12/09, mesclado na main `9e89c7d`): transformar em literais
independentes + teste anti-regressão.

### 3.4 Entitlements concedidos em produção sem enforcement conhecido

Concedidos a usuários reais, nenhum gate encontrado nos 6 repositórios varridos:
`monitorchannels` (32), `policyupdate` (7), `servicemonitor` (3), `aicontrol` (3), `payment` (1139 — provável
consumidor mobile), `aiuser` (30 — provável consumidor sufficit-ai), `mobile` (2), `expenseview`, `bankslippayerdata`,
`bankslipretention`, `bankslipmanage` (referências soltas), `contactdeny`, `groupcontacts`, `customergroup`.
**Ação:** varrer sufficit-ai, sufficit-cloud-mobile e asterisk-core; o que não tiver consumidor é linha morta
no banco (candidato a limpeza na fase de dados) ou entitlement a implementar.

### 3.5 Papéis redundantes ou vazios

- `telephonymanager` vs `telephonyadmin`: dois degraus quase sinônimos → fusão (PLAN §3.2).
- `salesrepresentative`, `exchangemanager` (papel), `FinancialRole` vs `FinancialManagerRole`: sem uso ou
  sobrepostos → decidir fusão/extinção na mesma janela de renames.

### 3.6 Dados e config (fase 2 do plano)

1. `userclaims`: rewrites das 4 chaves admin (~179 concessões / ~158 usuários distintos); split do telephonyadmin.
2. `roles`: rename `mobilecloudadministrator`→`vaultsecrets` (PK não muda; `userroles` intocada).
3. `applications.properties`: JSON `identity:client:roles` dos 5 machine clients.
4. `appsettings.Production.json` (identity): `RoleCapabilities` com chave nova (dupla durante a janela) + restart.
5. Revogação de tokens/sessões dos afetados (o `ClaimManagementService` já revoga ao tocar claims).
6. Queries de resíduo = 0 + smoke nos 6 produtos.

### 3.7 Código e docs (fase 1)

identity-core (classes/chaves/aliases `LegacyEntitlementKeys` + conformance), sufficit-endpoints (renames +
4 gates convertidos), sufficit-blazor (ContextView/SideBars/AuthorizeView), sufficit-web **recompilar e
publicar no IIS** (único usuário de `audioadmin`/`clientadmin`), sufficit-identity (testes, RoleCapabilities),
docs `docs/entitlements/*.md` + `roles.md` + decision record.

### 3.8 Já em voo (independente, retomável)

Piloto de permissões internas (branch `feat/internal-permissions-cache`): resolve entitlements no EndPoints com
cache por usuário; validado até o controle negativo (403 sem concessão); aguardava token operador e decisão de
deploy. A migração de nomes **não conflita** (o middleware lê as mesmas chaves; aliases cobrem a transição).

---

## 4. Ordem recomendada

1. **Fase 0** (sem janela): desacoplar chaves, aliases de leitura, decision record, varredura dos 3 repositórios restantes (§3.4).
2. **Fase 1** (código, leitura dupla): todos os itens §3.1/3.2/3.7.
3. **Fase 2** (janela ~1h, madrugada): §3.6 dados + verificação.
4. **Fase 3** (≥7 dias depois): remover aliases, strings duplicadas, backups, linhas mortas (§3.4/3.5).

Decisões pendentes com o Hugo: split vs renome único do `telephonyadmin` · fusão `telephonyadmin`→`telephonymanager`
vs `telephonyowner` · nome `vaultsecrets` · janela da fase 2.
