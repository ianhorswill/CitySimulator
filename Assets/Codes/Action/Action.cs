
using UnityEngine;

public class Action
{
    string actionName;
    object agent;
    object patient;
    Location location;
    Time time;

    public Action(string actionName, object agent, object patient, Location location, Time time)
    {
        this.actionName = actionName;
        this.agent = agent;
        this.patient = patient;
        this.location = location;
        this.time = time;
    }
}


public class ActionBase
{
    public string actionName;
    public int priority = 0;

    public virtual bool prerequisites(object agent, object patient, Location location, Time time)
    {
        return false;
    }

    public virtual void modifications(Action action)
    {
        
    }

    public virtual void triggers(Action action)
    {
        
    }
    
    public bool exec(object agent, object patient, Location location, Time time)
    {
        // Check if prereqs are satisfied
        if (prerequisites(agent, patient, location, time))
        {
            Action currAction = new Action(this.actionName, agent, patient, location, time);
            // TODO: what effect will the action have ?
            modifications(currAction);
            triggers(currAction);
            return true;
        }
        else
            return false;
    }
}

public class ActionTalk : ActionBase
{
    
    public ActionTalk(object agent, object patient)
    {
        
    }
    
    public int priority = 3;
    
    public override bool prerequisites(object agent, object patient, Location location, Time time)
    {
        // TODO: check the prereqs of this specific action 
        return false;
    }
    
    public void modifications(Action action)
    {
        // TODO: modify the world
    }
    
    public override void triggers(Action action)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}

