using Codes.Institution;

namespace Codes.Action.Actions
{
    public class ActionConstructInstitution : ActionType
    {
        public override string actionName => "ConstructInstitution";
        public override bool prerequisites(object agent, object patient, Location location, Time time)
        {
            return true;
        }
        
        // Agent is the construction company
        // Patient is the institution to be constructed
        public override void modifications(object agent, object patient, Location location, Time time)
        {
            (agent as ConstructionCompany).Build(patient as Institution.Institution, new Plot(location.x, location.y, new Space()));
        }

        public override void triggers(object agent, object patient, Location location, Time time)
        {
            throw new System.NotImplementedException();
        }
    }
}