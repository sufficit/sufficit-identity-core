# Telephony entitlements

8 entitlement(s). The identifiers below are published constants of
this library, not operational data.

| Key | Type | Name | ID | Self-context |
| --- | --- | --- | --- | --- |
| `audioadmin` | `AudioAdminDirective` | administrar áudios | `d05f3e2b-47bc-4af5-8cb3-28517be91b6f` | — |
| `audioupdate` | `AudioUpdateDirective` | atualizar áudio | `b327fb38-66f2-4063-9cd0-da1e4241d2d5` | — |
| `dialplanupdate` | `DialPlanUpdateDirective` | atualizar plano de discagem | `2c248f08-08d8-4396-9d52-83525366486d` | — |
| `monitorchannels` | `MonitorChannelsDirective` | ouvir canais de áudio | `7bc67d43-cb9a-46d9-a9eb-cc05561e0618` | — |
| `phonecalls` | `PhoneCallsDirective` | acesso a chamadas | `cf3c66ab-db24-48b6-8c28-4603540286de` | — |
| `portability` | `PortabilityDirective` | gerenciar processos de portabilidade | `fe921659-0606-4c43-982f-0c3baa5cf90a` | — |
| `telephonyadmin` | `TelephonyAdminDirective` | acesso a administração de telefonia | `09394ab4-8338-4662-a3d5-dd3a75324032` | — |
| `telephonyclient` | `TelephonyClientDirective` | acesso ao cliente de telefonia | `825e32b4-40d4-4f19-833c-7663bb9c26f7` | — |

A *self-context* entitlement resolves an empty stored context to the
principal's own identifier — read the empty value as *their own*, never as
*any*.

See [Entitlements](../entitlements.md) for the value format and comparison
rules.
