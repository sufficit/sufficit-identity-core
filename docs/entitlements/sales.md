# Sales entitlements

4 entitlement(s). The identifiers below are published constants of
this library, not operational data.

| Key | Type | Name | ID | Self-context |
| --- | --- | --- | --- | --- |
| `clientadmin` | `ClientAdminDirective` | controle de cliente | `9d7c9980-841a-4c93-bd64-8ade55a2f634` | — |
| `customergroup` | `CustomerGroupDirective` | acesso ao grupo de clientes | `77263022-11cb-4299-83fa-2496276e2f93` | — |
| `servicemonitor` | `ServiceMonitorDirective` | monitorar serviços de cliente | `362a9b8f-310c-4531-8103-7cca22a1d2f5` | — |
| `serviceupdate` | `ServiceUpdateDirective` | ativar servico de cliente | `3aa87d8a-bd7a-4111-b279-c396471d7b37` | — |

A *self-context* entitlement resolves an empty stored context to the
principal's own identifier — read the empty value as *their own*, never as
*any*.

See [Entitlements](../entitlements.md) for the value format and comparison
rules.
