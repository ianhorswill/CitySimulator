using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Random = System.Random;
using System.Linq;

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
//        sam = new Person("Sam", null);
//        jiawei = new Person("Jiawei", null);
//        sam.age = 22;
//        jiawei.age = 22;
        var initialSettlerTest = new List<Person>();
        Person p1 = new Person("Adam", null, 20, null, null, null, true);
        Person p2 = new Person("Eve", null, 20, p1, null, null, false);
        p1.sigOther = p2;
        initialSettlerTest.Add(p1);
        initialSettlerTest.Add(p2);
        ActionStatics.settlers = initialSettlerTest;
        ActionStatics.aliveResidents = initialSettlerTest;
    }

    Person RandomlyChoosePeople()
    {
        if (ActionStatics.aliveResidents.Count != 0)
            return ActionStatics.aliveResidents[
                ActionStatics.randomNumberGenerator.Next(ActionStatics.aliveResidents.Count)];
        else
            return null;
    }

    void Update()
    {
        // TODO: randomly select an action
        ActionType randAction = ActionLibrary.RandomlyChoose();
        
        // TODO: execute the action
        Person p1 = RandomlyChoosePeople();
        Person p2 = RandomlyChoosePeople();
        if (p1 == null || p2 == null)
        {
            return;
        }
        randAction.exec(p1, p2, new Location(new Random().Next(100), new Random().Next(100)), new Time(DateTime.Now.ToString("h:mm:ss tt")));
        
    }

}
