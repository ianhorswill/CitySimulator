using static RoleLibrary;
using System.Collections.Generic;

public class ActionTalk : ActionType
{
    public override string ActionName => "Talk";
    public override List<RoleTypeBase> Role_list => new List<RoleTypeBase>
    {
        GetRoleByName("RoleSpeaker"),
        GetRoleByName("RoleListener"),
        GetRoleByName("RoleSameLocation")
    };

    public override int Priority => 5;

    // Example code only:
    public override void Modifications(Action a)
    {
        var Listener = (Person)a["RoleListener"];
        //var topic = (Person)a["RoleConverstaionTopic"];
        //Listener.relationChangeBasedOnTopic(topic)
    }

    public override void PostExecute(Action a)
    {
        var Listener = (Person) a["RoleListener"];
        // TODO?: filter based on location, people nearby this conversation
        Action heard = ActionLibrary.InstantiateByName("Heard", "RoleHeard", Listener);
        if (heard != null)
        {
            ActionLibrary.ExecuteByName("Heard", heard);
        }
    }
}