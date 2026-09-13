# Plano de migração — eliminar o sufixo "admin" de entitlements e papéis derivados

Data: 2026-09-11 · Estado: PROPOSTA (aguarda validação de nomes e janela) · Escopo: sistema de permissões inteiro

## 1. Motivação

`administrator` é o administrador **global** do sistema. Entitlements e papéis derivados com sufixo
`admin` (`telephonyadmin`, `audioadmin`, `clientadmin`, `provisioningadmin`, `telephonyadmin` como
papel, `mobilecloudadministrator`) sugerem poder global quando na verdade são **por contexto** e por
domínio. Além do nome, `telephonyadmin` é um guarda-chuva: cobre DID, aliases de endpoints e tabelas
de custo com uma única chave. Objetivos:

1. Remover o sufixo `admin` de todo entitlement e papel **derivado** (o papel global `administrator`
   permanece — ele é a origem da confusão e o único que legitimately se chama admin).
2. Granular `telephonyadmin` por recurso.
3. Desacoplar a **chave do entitlement** do **nome do papel** (hoje são a mesma constante).

## 2. Inventário (fechado em 2026-09-11)

### 2.1 Entitlements com sufixo admin (identity-core)

| Classe | Chave | Usuários / claims | Papel derivado (IDRole) | Enforcement principal |
|---|---|---|---|---|
| `TelephonyAdminEntitlement` | `telephonyadmin` | 24 / 30 | `TelephonyAdminRole` | EndPoints: `DIDController` (FullSearch/AddOrUpdate/Remove), `EndPointController` (Aliases*), `BillingController`+`BalanceController` (por role string); Blazor: 8 páginas (`ContextView.Default<T>`) |
| `AudioAdminEntitlement` | `audioadmin` | 119 / 131 | `TelephonyRole` | Legado WebForms `sufficit-web` (`Audio.aspx.cs`); par do `audioupdate` (1034 usuários) |
| `ClientAdminEntitlement` | `clientadmin` | 9 / 12 | `SalesManagerRole` | Legado WebForms (`Clientes.aspx.cs`) |
| `ProvisioningAdminEntitlement` | `provisioningadmin` | 6 / 6 | `ProvisioningRole` | EndPoints: `ProvisioningController` (todas as rotas) |

### 2.2 Papéis com "admin"

| Papel | NormalizedName | Onde vive | Observação |
|---|---|---|---|
| `TelephonyAdminRole` | `telephonyadmin` | código (identity-core) | **Acoplado**: `TelephonyAdminEntitlement.NormalizedKey = TelephonyAdminRole.NormalizedName` — renomear um renomeia o outro implicitamente |
| `mobilecloudadministrator` | `mobilecloudadministrator` | **só banco/config** (sem classe) | 1 usuário em `userroles`; 5 machine clients com `identity:client:roles` em `applications.properties` (`sufficit_cloud_mobile_api`, `sufficit-endpoints-vault`, `sufficit_ai_vault`, `sufficit-endpoints-vault-v3`, 1 cliente fleet `credential-management`); `RoleCapabilities` em `appsettings.Production.json` (vault secrets read/manage/resolve) |
| `AdministratorRole` | `administrator` | código + banco (3 usuários) | **Mantém** — admin global, `FullAdministratorRoles` |

Cadeia de papéis de telefonia hoje: `telephony` < `telephonysupervisor` < `telephonymanager` <
`telephonyadmin` — dois degraus de "gestão" quase sinônimos é parte da confusão.

### 2.3 Achados latentes (corrigir durante a migração)

1. **Gates por role string nunca são satisfeitos pelos papéis derivados.** Tokens carregam `role`
   apenas de `userroles` no banco (papéis existentes: `administrator`, `manager`,
   `mobilecloudadministrator` — ver `GrantOperations.cs:149` e `AuthorizationController`:
   `GetRolesAsync`). A derivação entitlement→papel (`Utils.GetRoles`) existe só no consumidor
   (UI/UserPrincipal). Consequência: `[Authorize(Roles="telephonyadmin,salesmanager")]`
   (`BalanceController.Patch`, `BillingController.Cost*`) e `[Authorize(Roles =
   TelephonyManagerRole.NormalizedName)]` (`TimeConditionController`) não passam para portadores do
   entitlement correspondente. **Correção: converter para `EntitlementRequirement` na fase 1.**
2. **Acoplamento chave↔papel** (2.2): desacoplar antes de qualquer renomeação, senão a troca de
   nome do papel muda a chave persistida silenciosamente.
3. Machine clients **não** carregam chaves admin em entitlements (verificado em `applications`);
   apenas o papel `mobilecloudadministrator` em properties.
4. Casos de conformance (`EntitlementConformance`) e testes (`ClaimScopeMapTests`,
   `ManagementApplicationAuthorizationTests`, `ServiceAccountManagementTests`) referenciam as
   chaves/papéis antigos por string.

### 2.4 Precedentes a reutilizar

- Migrações anteriores com tabela de backup: `userclaims_bkp_entitlement_20260902`,
  `userclaims_bkp_directive_20260909`, `userroles_bkp_admin_20260902`.
- Dupla aceitação de nomes durante transição: `ProjectEntitlementClaimUnderBothNames` (STS) e
  `ClaimScopeMapOptions.ScopeSuccessors`. O mesmo padrão será aplicado a **chaves de entitlement**.

## 3. Taxonomia proposta

Convenção nova: `dominio.recurso.verbo` (minúsculas, pontos), com conjunto fechado de verbos
`read | write | update | delete | manage` — justificativa e matriz de correspondência no
[ESTUDO-20260912](../../sufficit-endpoints/docs/security/ESTUDO-20260912-endpoints-permissoes-read-write.md) §6.
O parser de claims separa no **primeiro**
`:` — chaves com ponto são seguras (`ClaimExtensions`, formato `JSON-array`, e o validador de escopo
do piloto aceitam `0x2E`). Role global `administrator` intocado.

### 3.1 Entitlements

| Atual | Proposto (chaves) | Cobertura |
|---|---|---|
| `telephonyadmin` | `telephony.did.read` + `telephony.did.write` | `DIDController` (todas), Blazor `DidDashboardPage`, `Free` |
| `telephonyadmin` | `telephony.endpoint.write` | `EndPointController` `Aliases*`, Blazor `PhoneSetup`, `AcdPage`, `IvrDashboard` |
| `telephonyadmin` | `telephony.cost.read` + `telephony.cost.write` | `BillingController` `Cost*`, `BalanceController.Patch` (após conversão de gate), Blazor `Calls` |
| `audioadmin` | `audio.write` | legado `Audio.aspx` (biblioteca de áudios: incluir/remover) |
| `clientadmin` | `sales.client.read` + `sales.client.write` | legado `Clientes.aspx` |
| `provisioningadmin` | `provisioning.read` + `provisioning.write` | `ProvisioningController` (todas) |

- `audioupdate` vira `audio.update` na mesma janela (verbo consistente); `dialplanupdate`,
  `monitorchannels` etc. seguem **fora de escopo** (não têm admin; já são granulares — uniformização de
  verbos opcional na fase 3).
- Split do `telephonyadmin`: cada concessão antiga `telephonyadmin:X` vira **5 linhas**
  (did.read/did.write/endpoint.write/cost.read/cost.write:X) — ~120 linhas para 24 usuários. Alias de
  leitura: chave antiga resolve como o **conjunto dos cinco** novos, então nada quebra antes da
  reescrita dos dados.

Alternativas registradas: (a) renome único `telephonyadmin`→`telephony.manage` sem divisão; (b) split em
3 chaves `.manage` (versão anterior deste plano). **Recomendação: split `.read`/`.write` em 5 chaves** —
é a granularidade pedida e separa leitura de escrita onde hoje a leitura sensível é implícita (evidência
de produção no ESTUDO-20260912, §2–§4).

### 3.2 Papéis

| Atual | Proposta | Justificativa |
|---|---|---|
| `TelephonyAdminRole` (`telephonyadmin`) | **fundir** em `TelephonyManagerRole` (`telephonymanager`) | Remove o degrau quase-duplicado da cadeia; `TimeConditionController` já é gated por `telephonymanager`; papéis devem ser grossos (docs/roles.md), a granularidade passa a viver nos entitlements |
| `mobilecloudadministrator` | renomear para `vaultsecrets` | Papel de confiança para resolução de segredos do vault; sem "admin", sem "cloud" (nome herdado de produto) |
| `administrator` / `manager` | mantém | globais, fora do escopo |

Se a fusão de papéis não for aceita: alternativa é renomear `telephonyadmin`→`telephonyowner`
(mesma semântica, outro nome). Decisão pendente (item 4.1).

## 4. Plano em fases

### Fase 0 — Preparação (sem mudança de comportamento) — ~1 dia

1. **Desacoplar chaves**: `TelephonyAdminEntitlement.Key` passa a literal `"telephonyadmin"` (hoje é
   `TelephonyAdminRole.NormalizedName`); idem conferir `ClientAdminEntitlement`. Teste impede
   regressão (chave != nome de papel).
2. **Resolver aliases de chave**: mapa `LegacyEntitlementKeys` no identity-core (chave antiga →
   conjunto de novas) aplicado na resolução de políticas (`ClaimsPrincipalExtensions.GetUserPolicies`
   e leitura persistida), com os mesmos testes de warning/ignore que existem hoje para chave
   desconhecida. Casos de conformance novos: "chave antiga concede os novos", "chave antiga em outro
   contexto não concede", etc.
3. Decision record (docs/decisions) registrando convenção `dominio.recurso.verbo` (`read`/`write`) e a deprecação.

### Fase 1 — Código (leitura dupla, escrita nova) — 2–3 dias

4. identity-core: novas classes/chaves (tabela 3.1), `LegacyKeys` nas antigas removidas das rotas;
   docs `docs/entitlements/*.md` atualizados com coluna "chave antiga".
5. sufficit-endpoints: `EntitlementRequirement(typeof(...))` trocados; **converter os 3 gates por
   role-string para `EntitlementRequirement`** (achado 2.3.1): `BillingController.Cost*` e
   `BalanceController.Patch` → `TelephonyCostManageEntitlement` (+`SalesManager` onde aplicável como
   entitlement `sales.*` ou manter role de banco), `TimeConditionController` → manter `telephonymanager`
   de banco (papel fundido).
6. sufficit-blazor: `ContextView.Default<T>` por página (tabela 3.1); `SideBarSnippet`
   (`IsInRole<TelephonyAdminRole>` → `TelephonyManagerRole`; constante `Roles` do Gateway atualizada).
7. sufficit-web (legado): recompilar `Audio.aspx`/`Clientes.aspx` com a nova identity-core e publicar
   (IIS) — quebra de compilação caso contrário.
8. sufficit-identity (STS/management): testes atualizados; `RoleCapabilities` com **ambas** as chaves
   (`mobilecloudadministrator` + `vaultsecrets`) durante a janela; conformance suite atualizada.

### Fase 2 — Dados (janela de manutenção, ~1h)

9. Backups: `userclaims_bkp_adminkeys_<data>`, `userroles_bkp_adminkeys_<data>`, snapshot de
   `roles` (linha `mobilecloudadministrator`) e das `properties` dos 5 clients.
10. Rewrites (transacional, com verificação de contagem antes/depois):
    - `userclaims`: `telephonyadmin:X` → 3 linhas novas; `audioadmin:X`→`audio.manage:X`;
      `clientadmin:X`→`sales.client.manage:X`; `provisioningadmin:X`→`provisioning.manage:X`.
    - `roles`: update `Name`/`NormalizedName` `mobilecloudadministrator`→`vaultsecrets` (PK id não
      muda; `userroles` não é tocada).
    - `applications.properties`: JSON `identity:client:roles` dos 5 clients → `vaultsecrets`.
    - Fusão de papel `telephonyadmin`→`telephonymanager`: nada no banco (papéis derivados não vivem
      em `userroles`); é troca de código.
    - `appsettings` por host: chave `RoleCapabilities` trocada + restart do host.
11. Revogar sessões/tokens dos usuários afetados (o `ClaimManagementService` já o faz ao tocar claims;
    para os 5 clients, revogar tokens de client_credentials por janela de expiração curta).
12. Verificação de resíduo (queries abaixo) = 0 + smoke:
    Blazor (DidDashboard, IVR, ACD, Calls, PhoneSetup, Free), legado (Audio, Clientes),
    Provisioning (Device CRUD), Balance/Billing como salesmanager.

### Fase 3 — Limpeza (após soak ≥ 7 dias / 1 ciclo de refresh)

13. Remover aliases `LegacyEntitlementKeys`, chaves duplicadas de `RoleCapabilities`, apagar tabelas
    de backup, fechar o decision record.

## 5. Verificação pós-migração (queries de resíduo)

```sql
SELECT SUBSTRING_INDEX(ClaimValue, ':', 1) k, COUNT(*) FROM userclaims
WHERE ClaimValue REGEXP '^(telephonyadmin|audioadmin|clientadmin|provisioningadmin):'
GROUP BY 1;                                        -- espera: 0 linhas
SELECT client_id FROM applications
WHERE properties LIKE '%mobilecloudadministrator%'; -- espera: 0 linhas
SELECT NormalizedName FROM roles
WHERE NormalizedName IN ('mobilecloudadministrator','telephonyadmin'); -- espera: 0 linhas
```

Monitorar warning existente "Ignoring invalid or unknown entitlement claim" (espera: zero por chave
antiga após fase 2; qualquer ocorrência indica emissor não migrado).

## 6. Riscos e rollback

| Risco | Mitigação | Rollback |
|---|---|---|
| Emissor antigo ainda grava chave antiga após fase 2 | aliases mantidos até fase 3; query de resíduo diária | restaurar backup tables + redeploy versão fase 1 (lê ambos) |
| Renome de papel muda chave de entitlement implicitamente | fase 0 desacopla chaves (teste dedicado) | n/a (pré-condição) |
| Gates convertidos (role-string → entitlement) mudam quem passa | auditar detentores antes (24 telephonyadmin, salesmanagers) e comparar matriz espera/obtém em homologação | revert do commit do controller |
| Legado `sufficit-web` sem deploy | compilar e publicar na fase 1 (bloqueia a liberação da identity-core nova) | manter versão anterior da dll |
| Tokens vivos com chaves antigas | aliases + revogação pós-rewrites | aliases cobrem sem revogação (menos limpo) |
| `vaultsecrets` em 5 machine clients falha silenciosa | introspecção/health por client pós-swap; segredo de vault é transitório (resolve sob demanda) | properties snapshot restore |

## 7. Decisões pendentes (dono: Hugo)

1. Split do `telephonyadmin` em 5 chaves `.read`/`.write` (recomendado) vs 3 chaves `.manage` (versão
   anterior) vs renome único `telephony.manage`.
2. Fusão `telephonyadmin`→`telephonymanager` (recomendado) vs renome `telephonyowner`.
3. Nome `vaultsecrets` para o papel de vault (alternativa: `vaultsecretstrustee`).
4. Janela da fase 2 (sugestão: madrugada, 1h, depois do deploy da fase 1 em todos os hosts).
