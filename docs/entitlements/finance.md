# Finance entitlements

12 entitlement(s). The identifiers below are published constants of
this library, not operational data.

| Key | Type | Name | ID | Self-context |
| --- | --- | --- | --- | --- |
| `balancetransfer` | `BalanceTransferEntitlement` | transferir saldo | `17f20ed119374f75b41931987a892ca0` | — |
| `balanceupdate` | `BalanceUpdateEntitlement` | realizar movimentação financeira | `cf50a644934748d9929618973a252a55` | — |
| `balanceview` | `BalanceViewEntitlement` | visualizar saldo | `1c8a1f496f3e49798885d52fc79f0dee` | — |
| `bankbillet` | `BankSlipEntitlement` | acesso a boletos | `1cea282f5b3645d685e6d1ad866d2b27` | — |
| `bankslipmanage` | `BankSlipManageEntitlement` | gerenciar boletos | `139e7b816b3c450a909ccd2b7b25f543` | — |
| `bankslippayerdata` | `BankSlipPayerDataEntitlement` | acessar dados do pagador de boletos | `9c1cc91856d1420cb6c035be272e768b` | — |
| `bankslipretention` | `BankSlipRetentionEntitlement` | gerenciar retenção de boletos | `7fb582fa089242c1baa20d53e20b29df` | — |
| `bankslipsettings` | `BankSlipSettingsEntitlement` | configurar boletos | `0416f398e9df41909f704ca015ee5806` | — |
| `expenseupdate` | `ExpenseUpdateEntitlement` | atualizar despesas | `245a76686916494692a0edc702b27137` | — |
| `expenseview` | `ExpenseViewEntitlement` | visualizar despesas | `606a66368577417aa027d6f4a8ed00ea` | — |
| `finance` | `FinanceEntitlement` | acesso ao financeiro | `20b48f3bd8394007bde438151740c6a9` | — |
| `payment` | `PaymentEntitlement` | efetuar pagamentos | `51a8108b4a1f4a53abed12dde40b238d` | — |

A *self-context* entitlement resolves an empty stored context to the
principal's own identifier — read the empty value as *their own*, never as
*any*.

See [Entitlements](../entitlements.md) for the value format and comparison
rules.
