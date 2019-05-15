// Actual Classes


using Codes.Action.Actions;
using Codes.Institution;

public class ActionGenerateInstitution : ActionType
{
    public int priority = 3;
    public Institution Institution;

    public override string ActionName => "GenerateInstitution";

    public override double Chance => 1.0;

    public override bool prerequisites(object agent, object patient, Location location, Time time)
    {
        // TODO: check the prereqs of this specific action 
        return true;
    }
    
    public override void modifications(object agent, object patient, Location location, Time time)
    {
        // TODO: modify the world
        Institution = InstitutionManager.GeneratorInstitution(agent as Person, new Plot(location.x, location.y, new Space()));
    }
    
    public override void triggers(object agent, object patient, Location location, Time time)
    {
        // TODO: call all the actions that will be triggered by this action
        
        // hiring process will be triggered by the generation of institution
        ActionInstitutionHiring actionInstitutionHiring = (ActionInstitutionHiring) ActionLibrary.GetActionByName("InstitutionHiring");
        actionInstitutionHiring.exec(agent, patient, location, time);
    }
}