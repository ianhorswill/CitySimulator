// Actual Classes


using System;
using Codes.Action.Actions;
using Codes.Institution;

public class ActionGenerateInstitution : ActionType
{
    public Institution Institution;

    public override string actionName => "GenerateInstitution";

    public override bool prerequisites(object agent, object patient, Location location, DateTime time)
    {
        // TODO: check the prereqs of this specific action 
        return true;
    }
    
    public override void modifications(object agent, object patient, Location location, DateTime time)
    {
        // TODO: modify the world
        Institution = InstitutionManager.GeneratorInstitution(agent as Person, new Plot(location.x, location.y, new Space()));
    }
    
    public override void triggers(object agent, object patient, Location location, DateTime time)
    {
        // TODO: call all the actions that will be triggered by this action
        
        // hiring process and construction will be triggered by the generation of institution
        ActionConstructInstitution actionConstructInstitution = (ActionConstructInstitution)
            ActionLibrary.GetActionByName("ConstructInstitution");
        ActionInstitutionHiring actionInstitutionHiring = (ActionInstitutionHiring) ActionLibrary.GetActionByName("InstitutionHiring");
        ConstructionCompany constructionCompany = InstitutionManager.GetRandomConstructionCompany();
        
        // build the institution
        actionConstructInstitution.exec(constructionCompany, Institution, location, time);
        // start hiring process
        actionInstitutionHiring.exec(agent, patient, location, time);
    }
}