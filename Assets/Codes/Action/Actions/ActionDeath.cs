
using System.Collections.Generic;

public class ActionDeath : ActionType
{
    public override string ActionName => "Death";
    public override double Chance => 1.0;

    public override List<RoleTypeBase> Role_list => throw new System.NotImplementedException();
}