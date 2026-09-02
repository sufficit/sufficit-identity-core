using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Sales
{
    /// <summary>
    /// Used to include on group Sales Representative 
    /// </summary>
    public class ServiceMonitorEntitlement : Entitlement
    {
        public const string UniqueID = "362a9b8f310c453181037cca22a1d2f5";

        public const string RoleID = SalesRepresentativeRole.UniqueID;

        public const string NormalizedKey = "servicemonitor";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "monitorar serviços de cliente";

        public override string Description => "Usado para incluir no Grupo de Representantes de vendas.";

        public override string Key => NormalizedKey;
    }
}
