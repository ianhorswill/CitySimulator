// Actual Classes

using Boo.Lang;

public class ActionHeard : ActionType
{
    public int priority = 3;

    public override string ActionName => "Heard";

    public override double Chance => 1.0;

    public List<RoleType<object>> roleList;

    public ActionHeard()
    {
        roleList.Add(new RoleHeard());
    }

    public override bool prerequisites(object agent, object patient, Location location, Time time)
    {
        // TODO: check the prereqs of this specific action 
        return true;
    }
    
    public override void modifications(object agent, object patient, Location location, Time time)
    {
        // TODO: modify the world
         
    }
    
    public override void triggers(object agent, object patient, Location location, Time time)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}