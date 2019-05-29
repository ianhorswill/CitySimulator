// Actual Classes


using System;
using System.Collections.Generic;
using Codes.Action.Actions;
using Codes.Institution;
using UnityEngine;

public class ActionGenerateInstitution : ActionType
{
    public Institution Institution;

    public override string ActionName => "GenerateInstitution";

    public override double Chance => 1.0;
//    public override int Priority => 6;

    public override List<RoleTypeBase> Role_list => new List<RoleTypeBase>()
    {
        RoleLibrary.GetRoleByName("RoleCEO"),
        RoleLibrary.GetRoleByName("RoleConstructionCompany")
    };

    public override void Modifications(Action a)
    {
        Debug.Log("ActionGenerateInstitution");
        Institution ins = InstitutionManager.GeneratorInstitution(
            (Person) a["RoleCEO"],
            InstitutionManager.GetRandomType(),
            Space.Singleton.get_random_plot());
        
        ((ConstructionCompany) a["RoleConstructionCompany"]).Build(ins);
    }
}