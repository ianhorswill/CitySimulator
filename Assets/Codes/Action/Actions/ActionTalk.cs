using System.Collections.Generic;

public class ActionTalk : ActionType
{
    public override string ActionName => "Talk";
    public override List<RoleTypeBase> Role_list => new List<RoleTypeBase>
    {
        ActionLibrary.roleLibrary.GetRoleByName("RoleSpeaker"),
        ActionLibrary.roleLibrary.GetRoleByName("RoleListener"),
        ActionLibrary.roleLibrary.GetRoleByName("RoleSameLocation")
    };

    public override int Priority => 5;

    public override void Triggers(Action a)
    {
        Person Listener = (Person) a.roles[1].GetBindingUntyped();
        List<Person> new_collection = new List<Person> { Listener };
        List<RoleBase> role_map = new List<RoleBase>();
        ActionType act_heard = ActionLibrary.GetActionByName("Heard");
        RoleType<Person> role_heard = (RoleType<Person>) act_heard.Role_list[0];
        Role<Person> filled = role_heard.GetRole(role_map, new_collection);
        if (filled != null)
        {
            role_map.Add(filled);
            Action done = new Action(ActionName, Simulator.CurrentTime, role_map);
            // Add action to list of all completed actions
            act_heard.Execute(done);
        }
    }
}