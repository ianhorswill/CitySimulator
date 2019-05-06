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

public class ActionSimulator : MonoBehaviour
{
    private Person sam;
    private Person jiawei;
    
    void Start()
    {
        Debug.Log("Action Simulation Start");
        sam = new Person("Sam", null);
        jiawei = new Person("Jiawei", null);
        sam.age = 22;
        jiawei.age = 22;
    }

    void Update()
    {
        // TODO: randomly select an action
        ActionType randAction = ActionLibrary.RandomlyChoose();
        
        // TODO: execute the action
        randAction.exec(sam, jiawei, new Location(new Random().Next(100), new Random().Next(100)), new Time(DateTime.Now.ToString("h:mm:ss tt")));
        
    }

}
