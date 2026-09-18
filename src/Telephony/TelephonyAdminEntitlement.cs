using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Telephony
{
    public class TelephonyAdminEntitlement : Entitlement
    {
        public const string UniqueID = "09394ab483384662a3d5dd3a75324032";        

        public const string RoleID = TelephonyAdminRole.UniqueID;

        // 2026-09-12: chave deliberadamente DESacoplada do nome do papel. Era
        // `TelephonyAdminRole.NormalizedName`, o que fazia renomear o papel
        // mudar silenciosamente a chave persistida nas claims. Migração:
        // PLAN-20260911-admin-suffix-migration.md (fase 0), decisão 0006.
        public const string NormalizedKey = "telephonyadmin";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "acesso a administração de telefonia";

        public override string Key => NormalizedKey;
    }
}
