# Sales entitlements

4 entitlement(s). The identifiers below are published constants of
this library, not operational data.

| Key | Type | Name | ID | Self-context |
| --- | --- | --- | --- | --- |
| `clientadmin` | `ClientAdminEntitlement` | controle de cliente | `9d7c9980841a4c93bd648ade55a2f634` | — |
| `customergroup` | `CustomerGroupEntitlement` | acesso ao grupo de clientes | `7726302211cb429983fa2496276e2f93` | — |
| `servicemonitor` | `ServiceMonitorEntitlement` | monitorar serviços de cliente | `362a9b8f310c453181037cca22a1d2f5` | — |
| `serviceupdate` | `ServiceUpdateEntitlement` | ativar servico de cliente | `3aa87d8abd7a4111b279c396471d7b37` | — |

A *self-context* entitlement resolves an empty stored context to the
principal's own identifier — read the empty value as *their own*, never as
*any*.

See [Entitlements](../entitlements.md) for the value format and comparison
rules.
