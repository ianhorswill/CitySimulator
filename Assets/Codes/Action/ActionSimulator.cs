using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = System.Random;

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

public class Action
{
    string actionName;
    object agent;
    object patient;
    Location location;
    Time time;
    // More role names...
    // Dict fill?

    public Action(string actionName, object agent, object patient, Location location, Time time)
    {
        this.actionName = actionName;
        this.agent = agent;
        this.patient = patient;
        this.location = location;
        this.time = time;
    }
    
    public override string ToString()
    {
        String res = ((Person) agent).name + " " + actionName + " to " + ((Person) patient).name + " at " + location.ToString() + ", " + time.timestamp;
        
        return res;
    }
}

public abstract class ActionType
{
    public abstract string actionName { get;}
    public int priority = 0;
    // role desc
    //  - name
    //  - attr (e.g. must like the person who -stored in other role- ...) | prolog-like (enum, flags..)
    //  - lambda (filter) <-- delegate types... vs func<int, string> | data driven

    public abstract bool prerequisites(object agent, object patient, Location location, Time time);
    // LAMBDA??

    public abstract void modifications(object agent, object patient, Location location, Time time);

    public abstract void triggers(object agent, object patient, Location location, Time time);
    
    public bool exec(object agent, object patient, Location location, Time time)
    {
        if (prerequisites(agent, patient, location, time))
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
