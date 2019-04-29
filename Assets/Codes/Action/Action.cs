
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
        return true;
    }

    public virtual void modifications(object agent, object patient, Location location, Time time)
    {}

    public virtual void triggers(object agent, object patient, Location location, Time time)
    {}
    
    public bool exec(object agent, object patient, Location location, Time time)
    {
        if (prerequisites(agent, patient, location, time))
        {
            Action currAction = new Action(this.actionName, agent, patient, location, time);
            modifications(agent, patient, location, time);
            triggers(agent, patient, location, time);
            return true;
        }
        else
            return false;
    }
}

public class ActionTalk : ActionBase
{
    public string actionName = 'Talk';
    public int priority = 3;
    
//     public override bool prerequisites(object agent, object patient, Location location, Time time)
//     {
//         // TODO: check the prereqs of this specific action 
//         return true;
//     }
    
//     public override void modifications(object agent, object patient, Location location, Time time)
//     {
//         // TODO: modify the world
//     }
    
//     public override void triggers(object agent, object patient, Location location, Time time)
//     {
//         // TODO: call all the actions that will be triggered by this action
//     }
}
