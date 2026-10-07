# Constantes canônicas dos entitlements de clientes

Solicitação: usar a constante da chave de cada entitlement em vez de repetir strings no catálogo Blazor.

TelephonyClientEntitlement e AIUserEntitlement já tinham NormalizedKey. Adicionada a mesma constante em PhoneCallsEntitlement, MonitorChannelsEntitlement, AudioUpdateEntitlement, DialPlanUpdateEntitlement, PaymentEntitlement, BalanceViewEntitlement e BankSlipEntitlement. Key retorna a constante. Valores persistidos (inclusive bankbillet), GUIDs e roles preservados.

GuidedAccessCatalog e testes associados usam as nove constantes. Regra registrada no AGENTS.md do Blazor. balanceview permanece nos dois presets, conforme correção anterior.

Validação:
- dotnet build src/Sufficit.Identity.Core.csproj --no-restore -v minimal: aprovado em netstandard2.0, net7.0 e net10.0, com avisos existentes.
- dotnet test tests/Sufficit.Identity.Core.Tests/Sufficit.Identity.Core.Tests.csproj --no-restore -v minimal: 41 passaram.
- dotnet test tests/Sufficit.Blazor.Tests.csproj --no-restore --filter 'FullyQualifiedName~GuidedPoliciesTests|FullyQualifiedName~GuidedAccessBalanceViewTests' -v minimal: 45 passaram; recompilação do consumidor aprovada, com avisos existentes.

Entrega local em identity-core e blazor, sem commit/push/deploy. Publicação continua aguardando a decisão já solicitada sobre alterações alheias em dependências locais.
