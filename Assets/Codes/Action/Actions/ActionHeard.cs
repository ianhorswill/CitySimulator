// Actual Classes
public class ActionHeard : ActionType
{
    public override string actionName => "Heard";

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