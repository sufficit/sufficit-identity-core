# Finance entitlements

12 entitlement(s). The identifiers below are published constants of
this library, not operational data.

| Key | Type | Name | ID | Self-context |
| --- | --- | --- | --- | --- |
| `balancetransfer` | `BalanceTransferDirective` | transferir saldo | `17f20ed1-1937-4f75-b419-31987a892ca0` | — |
| `balanceupdate` | `BalanceUpdateDirective` | realizar movimentação financeira | `cf50a644-9347-48d9-9296-18973a252a55` | — |
| `balanceview` | `BalanceViewDirective` | visualizar saldo | `1c8a1f49-6f3e-4979-8885-d52fc79f0dee` | — |
| `bankbillet` | `BankSlipDirective` | acesso a boletos | `1cea282f-5b36-45d6-85e6-d1ad866d2b27` | — |
| `bankslipmanage` | `BankSlipManageDirective` | gerenciar boletos | `139e7b81-6b3c-450a-909c-cd2b7b25f543` | — |
| `bankslippayerdata` | `BankSlipPayerDataDirective` | acessar dados do pagador de boletos | `9c1cc918-56d1-420c-b6c0-35be272e768b` | — |
| `bankslipretention` | `BankSlipRetentionDirective` | gerenciar retenção de boletos | `7fb582fa-0892-42c1-baa2-0d53e20b29df` | — |
| `bankslipsettings` | `BankSlipSettingsDirective` | configurar boletos | `0416f398-e9df-4190-9f70-4ca015ee5806` | — |
| `expenseupdate` | `ExpenseUpdateDirective` | atualizar despesas | `245a7668-6916-4946-92a0-edc702b27137` | — |
| `expenseview` | `ExpenseViewDirective` | visualizar despesas | `606a6636-8577-417a-a027-d6f4a8ed00ea` | — |
| `finance` | `FinanceDirective` | acesso ao financeiro | `20b48f3b-d839-4007-bde4-38151740c6a9` | — |
| `payment` | `PaymentDirective` | efetuar pagamentos | `51a8108b-4a1f-4a53-abed-12dde40b238d` | — |

A *self-context* entitlement resolves an empty stored context to the
principal's own identifier — read the empty value as *their own*, never as
*any*.

See [Entitlements](../entitlements.md) for the value format and comparison
rules.
