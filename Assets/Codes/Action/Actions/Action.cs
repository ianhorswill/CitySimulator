using System;
using UnityEngine;

public class Action
{
    string actionName;
    object agent;
    object patient;
    Location location;
    Time time;
    // Each of these should be a role, provided in some list or dict that gets filled in...
    // role desc:
    //  - name
    //  - type (object type)
    //  - attr (e.g. must like the person who -stored in other role- ...) | prolog-like (enum, flags..)
    //  - lambda (filter) <-- delegate types... vs func<int, string> | data driven

    public Action(string actionName, object agent, object patient, Location location, Time time)
    {
        this.actionName = actionName;
        this.agent = agent;
        this.patient = patient;
        this.location = location;
        this.time = time;
    }
    
    // USED FOR DEBUG.LOG - need an alternative...
    public override string ToString()
    {
        String res = ((Person) agent).name + " and " + ((Person) patient).name + " " + actionName + " at " + location.ToString() + ", " + time.timestamp;
        return res;
    }
}