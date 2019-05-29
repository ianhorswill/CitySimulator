// Actual Classes

using System.Collections.Generic;
using UnityEngine;

// DEPRECATED - see code in ActionLibrary.cs    -ian

//public class ActionGiveBirth : ActionType
//{
//    public override string ActionName => "GiveBirth";
    
//    public override int Priority => 2;
//    public override double Chance => 1.0;

//    public override List<RoleTypeBase> Role_list => new List<RoleTypeBase>
//    {
//        RoleLibrary.GetRoleByName("RoleBioMother")
//    };
    
//    public override void Modifications(Action a)
//    {
//        var BioMother = (Person)a["RoleBioMother"];
//        var BioFather = BioMother.sigOther;
//        Person baby = Person.createChild(BioMother, BioFather);
//        PersonTown.Singleton.aliveResidents.Add(baby);
//    }
//}