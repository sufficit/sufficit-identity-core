# Telephony entitlements

8 entitlement(s). The identifiers below are published constants of
this library, not operational data.

| Key | Type | Name | ID | Self-context |
| --- | --- | --- | --- | --- |
| `audioadmin` | `AudioAdminEntitlement` | administrar áudios | `d05f3e2b47bc4af58cb328517be91b6f` | — |
| `audioupdate` | `AudioUpdateEntitlement` | atualizar áudio | `b327fb3866f240639cd0da1e4241d2d5` | — |
| `dialplanupdate` | `DialPlanUpdateEntitlement` | atualizar plano de discagem | `2c248f0808d843969d5283525366486d` | — |
| `monitorchannels` | `MonitorChannelsEntitlement` | ouvir canais de áudio | `7bc67d43cb9a46d9a9ebcc05561e0618` | — |
| `phonecalls` | `PhoneCallsEntitlement` | acesso a chamadas | `cf3c66abdb2448b68c284603540286de` | — |
| `portability` | `PortabilityEntitlement` | gerenciar processos de portabilidade | `fe92165906064c43982f0c3baa5cf90a` | — |
| `telephonyadmin` | `TelephonyAdminEntitlement` | acesso a administração de telefonia | `09394ab483384662a3d5dd3a75324032` | — |
| `telephonyclient` | `TelephonyClientEntitlement` | acesso ao cliente de telefonia | `825e32b440d44f19833c7663bb9c26f7` | — |

A *self-context* entitlement resolves an empty stored context to the
principal's own identifier — read the empty value as *their own*, never as
*any*.

See [Entitlements](../entitlements.md) for the value format and comparison
rules.
