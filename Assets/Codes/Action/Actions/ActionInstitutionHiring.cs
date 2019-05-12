using System;

namespace Codes.Action.Actions
{
    public class ActionInstitutionHiring : ActionType
    {
        public override string actionName => "InstitutionHiring";
        public override bool prerequisites(object agent, object patient, Location location, DateTime time)
        {
            // TODO: check the prereqs of this specific action 
            return true;
        }
    
        public override void modifications(object agent, object patient, Location location, DateTime time)
        {
            // TODO: modify the world
//            (agent as Institution.Institution)?.Hiring(patient as Person);
            
        }
    
        public override void triggers(object agent, object patient, Location location, DateTime time)
        {
            // TODO: call all the actions that will be triggered by this action
        }
    }
}