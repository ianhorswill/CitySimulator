using System.Collections.Generic;
using UnityEngine;

namespace Codes.Action.Actions
{
    public class ActionInstitutionHiring : ActionType
    {
        public override string ActionName => "InstitutionHiring";
        public override double Chance => 1.0;
//        public override int Priority => 5;

        public override List<RoleTypeBase> Role_list => new List<RoleTypeBase>()
        {
            RoleLibrary.GetRoleByName("RoleInstitution"),
            RoleLibrary.GetRoleByName("RoleEmployee")
        };

        public override void Modifications(global::Action a)
        {
            Debug.Log("ActionInstitutionHiring");
            ((Institution.Institution) a["RoleInstitution"]).Hiring((Person)a["RoleEmployee"]);
        }
    }
}