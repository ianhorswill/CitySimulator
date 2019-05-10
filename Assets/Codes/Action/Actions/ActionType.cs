using UnityEngine;

public abstract class ActionType
{
    public abstract string actionName { get;}
    public int priority = 0;

    // instead, pass in a list of roles that can be filled... this then means that prereq is not
    // something you write, it is some list (data) you provide that gets run by a default
    // prereq check function
    public abstract bool prerequisites(object agent, object patient, Location location, Time time);
    
    // filled out per class to actuate creation, modification, and destruction of other objects
    // in the game world...
    public abstract void modifications(object agent, object patient, Location location, Time time);
    
    // Actions that should be triggered after... need to revisit this because of role based
    // prereqs...
    public abstract void triggers(object agent, object patient, Location location, Time time);
    
    public bool exec(object agent, object patient, Location location, Time time)
    {
        if (prerequisites(agent, patient, location, time))  // needs to come before exec
        {
            // check roles...
            Action currAction = new Action(this.actionName, agent, patient, location, time);
            //Debug.Log("Prerequisite check succeed...");
            Logger.Log("actions", currAction.ToString());
            modifications(agent, patient, location, time);
            triggers(agent, patient, location, time);
            return true;
        }
        else
        {
            //Debug.Log("Prerequisite check failed...");
            return false;
        }
         
    }
}