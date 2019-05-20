using System;
using System.Collections.Generic;
using Codes.Institution;

namespace Codes.Action.Actions
{
    public class ActionConstructInstitution : ActionType
    {
        public override string ActionName => "ConstructInstitution";

        public override List<RoleTypeBase> Role_list => throw new NotImplementedException();
    }
}