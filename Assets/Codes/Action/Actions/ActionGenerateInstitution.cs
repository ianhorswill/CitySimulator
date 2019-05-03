// Actual Classes


using Institution;

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
        Institution.Institution ins = InstitutionManager.GeneratorInstitution(((Person) agent).name, location.ToString());
    }
    
    public override void triggers(object agent, object patient, Location location, Time time)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}