using Sufficit.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sufficit.Finance
{
    public class PaymentDirective : Directive
    {
        public const string UniqueID = "51a8108b-4a1f-4a53-abed-12dde40b238d";
        public const string RoleID = "547c85dc-15fc-4730-9377-98e1bacd0899";

        public override Guid ID => Guid.Parse(UniqueID);

        public override Guid IDRole => Guid.Parse(RoleID);

        public override string Name => "efetuar pagamentos";

        public override string Key => "payment";
    }
}
