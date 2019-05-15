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
        Space space = new Space();
        space.Initialize();
        Institution = InstitutionManager.GeneratorInstitution(agent as Person,
            patient as string,
            space.get_random_plot());
    }
    
    public override void triggers(object agent, object patient, Location location, DateTime time)
    {
        // TODO: call all the actions that will be triggered by this action
        
        // hiring process and construction will be triggered by the generation of institution
        ActionConstructInstitution actionConstructInstitution = (ActionConstructInstitution)
            ActionLibrary.GetActionByName("ConstructInstitution");
        ActionInstitutionHiring actionInstitutionHiring = (ActionInstitutionHiring) ActionLibrary.GetActionByName("InstitutionHiring");
        
        // get a construction company from Institution Manager
        ConstructionCompany constructionCompany = InstitutionManager.GetRandomConstructionCompany();
        
        // build the institution
        actionConstructInstitution.exec(constructionCompany, Institution, location, time);
        // start hiring process
        actionInstitutionHiring.exec(Institution, Person.generateRandomPerson(), location, time);
    }
}