using System;
using Codes.Institution;

namespace Codes.Action.Actions
{
    public class ActionConstructInstitution : ActionType
    {
        public override string actionName => "ConstructInstitution";
        public override bool prerequisites(object agent, object patient, Location location, DateTime time)
        {
            return true;
        }
        
        // Agent is the construction company
        // Patient is the institution to be constructed
        public override void modifications(object agent, object patient, Location location, DateTime time)
        {
            (agent as ConstructionCompany).Build(patient as Institution.Institution,
                (patient as Institution.Institution).location);
        }

        public override void triggers(object agent, object patient, Location location, DateTime time)
        {
            // TODO: construction triggers
        }
    }
}