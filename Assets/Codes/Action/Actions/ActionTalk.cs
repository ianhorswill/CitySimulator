using System;
using UnityEngine;

public class ActionTalk : ActionType
{
    public override string actionName => "Talk";

    public override bool prerequisites(object agent, object patient, Location location, DateTime time)
    {
        // TODO: check the prereqs of this specific action 
        return true;
    }
    
    public override void modifications(object agent, object patient, Location location, DateTime time)
    {
        // TODO: modify the world
         
    }
    
    public override void triggers(object agent, object patient, Location location, DateTime time)
    {
        // TODO: call all the actions that will be triggered by this action
        //Debug.Log("Trigger ActionHeard");
        ActionHeard actionHeard = (ActionHeard) ActionLibrary.GetActionByName("Heard");
        actionHeard.exec(agent, patient, location, time);
        actionHeard.exec(patient, agent, location, time);
    }
}