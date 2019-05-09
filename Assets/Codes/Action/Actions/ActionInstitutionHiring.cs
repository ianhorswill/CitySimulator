namespace Codes.Action.Actions
{
    public class ActionInstitutionHiring : ActionType
    {
        public int priority = 3;
        public override string actionName => "InstitutionHiring";
        public override double chance => 1.0;

        public override bool prerequisites(object agent, object patient, Location location, Time time)
        {
            // TODO: check the prereqs of this specific action 
            return true;
        }
    
        public override void modifications(object agent, object patient, Location location, Time time)
        {
            // TODO: modify the world
//            (agent as Institution.Institution)?.Hiring(patient as Person);
            
        }
    
        public override void triggers(object agent, object patient, Location location, Time time)
        {
            // TODO: call all the actions that will be triggered by this action
        }
    }
}