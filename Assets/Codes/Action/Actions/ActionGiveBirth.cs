// Actual Classes

using System.Collections.Generic;
using UnityEngine;

public class ActionGiveBirth : ActionType
{
    public override string ActionName => "GiveBirth";

    public override double Chance => 1.0;

    public override List<RoleTypeBase> Role_list => throw new System.NotImplementedException();
}