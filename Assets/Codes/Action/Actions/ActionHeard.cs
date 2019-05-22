using System.Collections.Generic;

public class ActionHeard : ActionType
{
    public override string ActionName => "Heard";
    public override List<RoleTypeBase> Role_list => new List<RoleTypeBase>
    {
        RoleLibrary.GetRoleByName("RoleHeard")
    };

    public override int Priority => 6;
    public override double Chance => 0.5;
}