// Actual Classes


using Codes.Institution;

public class ActionGenerateInstitution : ActionType
{
    public int priority = 3;

    public override string actionName => "GenerateInstitution";

    public override bool prerequisites(object agent, object patient, Location location, Time time)
    {
        // TODO: check the prereqs of this specific action 
        return true;
    }
    
    public override void modifications(object agent, object patient, Location location, Time time)
    {
        // TODO: modify the world
        Institution ins = InstitutionManager.GeneratorInstitution(agent as Person, new Plot(location.x, location.y));
    }
    
    public override void triggers(object agent, object patient, Location location, Time time)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}