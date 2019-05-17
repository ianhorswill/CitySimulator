using System.Collections.Generic;

public class ActionHeard : ActionType
{
    public override string ActionName => "Heard";
    public override List<RoleTypeBase> Role_list => new List<RoleTypeBase> { ActionLibrary.roleLibrary.GetRoleByName("RoleHeard") };

    public override int Priority => 5;
    public override double Chance => 0.5;

    public override void Modifications(Action a)
    {
        throw new System.NotImplementedException();
    }

    public override void Triggers(Action a)
    {
        throw new System.NotImplementedException();
    }
}