using System.Collections.Generic;

namespace Codes.Action.Actions
{
    public class ActionInstitutionHiring : ActionType
    {
        public override string ActionName => "InstitutionHiring";
        public override double Chance => 1.0;

        public override List<RoleTypeBase> Role_list => throw new System.NotImplementedException();
    }
}