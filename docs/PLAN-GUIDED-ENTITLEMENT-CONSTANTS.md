# Constantes dos entitlements de acesso guiado

Objetivo: expor NormalizedKey nos sete entitlements do preset Blazor que ainda usam literais em Key, sem alterar valores persistidos ou GUIDs.

1. [concluído] Adicionar constantes e alinhar Key, conforme TelephonyClientEntitlement/AIUserEntitlement.
2. [concluído] Validar build e testes de identidade/presets; registrar atividade.

Classes: PhoneCalls, MonitorChannels, AudioUpdate, DialPlanUpdate, Payment, BalanceView e BankSlip.
Validação: dotnet build src/Sufficit.Identity.Core.csproj --no-restore; testes de identidade e testes GuidedPolicies/GuidedAccessBalanceView no Blazor.
Publicação permanece pendente da decisão sobre alterações alheias nas dependências do Blazor.

Concluído: build dos três frameworks aprovado (avisos existentes), 41 testes de identidade e 45 testes direcionados no consumidor Blazor passaram. Sem publicação.
