using System.Collections.Generic;

public class ActionTalk : ActionType
{
    public override string ActionName => "Talk";
    public override List<RoleTypeBase> Role_list => throw new System.NotImplementedException();

    //public override double Chance => 1.0;
    public override int Priority => 5;

    public override void Modifications(Action a)
    {
        throw new System.NotImplementedException();
    }

    public override void Triggers(Action a)
    {
        throw new System.NotImplementedException();
    }
}