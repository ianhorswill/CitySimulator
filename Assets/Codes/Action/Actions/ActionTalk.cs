using UnityEngine;

public class ActionTalk : ActionType
{
    
    public int priority = 3;

    public override string ActionName => "Talk";

    public override double Chance => 1.0;

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
        //Debug.Log("Trigger ActionHeard");
        ActionHeard actionHeard = (ActionHeard) ActionLibrary.GetActionByName("Heard");
        actionHeard.exec(agent, patient, location, time);
        actionHeard.exec(patient, agent, location, time);
    }
}