using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = System.Random;

//  Pseudo Classes
public class Location
{
    public int x;
    public int y;

    public Location(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "(x = " + x + ", y = " + y + ")";
    }
}

public class Time
{
    public string timestamp;

    public Time(string timestamp)
    {
        this.timestamp = timestamp;
    }
}

public class Person
{
    public string name;

    public Person(string name)
    {
        this.name = name;
    }
}

// Actual Classes
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
        String res = ((Person) agent).name + " " + actionName + " " + ((Person) patient).name + " at " + location.ToString() + ", " + time.timestamp;
        return res;
    }
}

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
            Debug.Log("Prerequisite check succeed...");
            Debug.Log(currAction.ToString());
            modifications(agent, patient, location, time);
            triggers(agent, patient, location, time);
            return true;
        }
        else
        {
            Debug.Log("Prerequisite check failed...");
            return false;
        }
         
    }
}

public class ActionTalk : ActionType
{
    
    public int priority = 3;

    public override string actionName
    { get { return "Talk"; } }

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
         Debug.Log("Trigger ActionHeard");
         ActionHeard actionHeard = (ActionHeard) ActionLibrary.GetActionByName("Heard");
         actionHeard.exec(agent, patient, location, time);
         actionHeard.exec(patient, agent, location, time);
     }
}

public class ActionHeard : ActionType
{
    public int priority = 3;

    public override string actionName
    {
        get { return "Heard"; }
    }

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
    }
}

public class ActionSimulator : MonoBehaviour
{
    private Person sam;
    private Person jiawei;
    
    void Start()
    {
        Debug.Log("Action Simulation Start");
        sam = new Person("Sam");
        jiawei = new Person("Jiawei");
    }

    void Update()
    {
        // TODO: randomly select an action
        ActionType randAction = ActionLibrary.RandomlyChoose();
        
        // TODO: execute the action
        randAction.exec(sam, jiawei, new Location(new Random().Next(100), new Random().Next(100)), new Time(DateTime.Now.ToString("h:mm:ss tt")));
        
    }

}
