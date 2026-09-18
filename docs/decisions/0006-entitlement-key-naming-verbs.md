# 0006 — Entitlement keys are `domínio.recurso.verbo` and never role names

Date: 2026-09-12 · Status: aceita · Supersedes: nada (estende 0001) ·
Contexto completo: `PLAN-20260911-admin-suffix-migration.md`, `MAPA-20260911-permissoes.md`,
`sufficit-endpoints/docs/security/ESTUDO-20260912-endpoints-permissoes-read-write.md`

## Contexto

O sufixo `admin` em entitlements e papéis derivados (`telephonyadmin`, `audioadmin`, `clientadmin`,
`provisioningadmin`, papel `telephonyadmin`, `mobilecloudadministrator`) confunde-se com
`administrator`, que é o administrador **global**. Além do nome, a chave de um entitlement estava
acoplada ao nome do papel pela mesma constante (`TelephonyAdminEntitlement.NormalizedKey =
TelephonyAdminRole.NormalizedName`), e `telephonyadmin` é um guarda-chuva que cobre DID, aliases,
custos e valores com uma chave só.

## Decisão

1. **Convenção de chave**: `dominio.recurso.verbo`, minúsculas, com conjunto fechado de verbos
   `read | write | update | delete | manage`.
   - `read` — consultar, listar, buscar. Leituras sensíveis ganham `.read` **próprio**; leitura não é
     mais implícita em quem escreve.
   - `write` — mutação padrão (criar/atualizar/remover).
   - `update` / `delete` — apenas quando separar mutação de exclusão tiver efeito prático.
   - `manage` — operações irreversíveis/impacto global, usado com parcimônia; não é um sinônimo de
     "admin" disfarçado.
2. **Desacoplamento chave↔papel**: nenhuma chave de entitlement pode referenciar constante de papel.
    O `telephonyadmin` foi trocado por literal em 2026-09-12 (fase 0 do plano), guardado por
    `EntitlementKeyDecouplingTests`.
3. **Sem sufixo `admin`** em entitlements e papéis derivados; `administrator` (global) é o único que
    mantém o nome — é a origem do significado.
4. RENOVAÇÕES de chaves antigas passam por alias de leitura (`LegacyEntitlementKeys`) durante a
   janela de migração, no mesmo modelo da dupla `directive`/`entitlements`.

## Consequências

- O split do `telephonyadmin` gera 5 chaves: `telephony.did.read`, `telephony.did.write`,
  `telephony.endpoint.write`, `telephony.cost.read`, `telephony.cost.write` (~120 concessões novas
  para 24 usuários).
- Novos gates no sufficit-endpoints seguem a convenção desde já (ex.: `finance.balance.read`,
  `system.read`, `logging.write`, `gateway.openai.write` — ver estudo §3/§4).
- Renomear papéis deixa de ter efeito colateral sobre claims persistidas.
