using System;
using System.Collections.Generic;
using Codes.Institution;
using UnityEngine;

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

public class ActionSimulator : SimulatorComponent
{
    private Person sam;
    private Person jiawei;
    
    public override void Initialize()
    {
        //Log("Start");
        //Debug.Log("Action Simulation Start");
//        sam = new Person("Sam", null);
//        jiawei = new Person("Jiawei", null);
//        sam.age = 22;
//        jiawei.age = 22;
    }

    Person RandomlyChoosePeople()
    {
        return PersonTown.Singleton.aliveResidents.RandomElement();
    }

    public override void Step()
    {
        if (PersonTown.Singleton.aliveResidents.Count == 0)
            // Town is dead
            return;

        // TODO: randomly select an action
        ActionType randAction = ActionLibrary.RandomlyChoose();

        switch (randAction.actionName)
        {
            case "GenerateInstitution":
                randAction.exec(RandomlyChoosePeople(), InstitutionManager.GetRandomType(),
                    new Location(Random.Integer(100), Random.Integer(100)), 
                    Simulator.CurrentTime);
                break;
            // ignore these two action types in the simulator
            case "InstitutionHiring":
                break;
            case "ConstructInstitution":
                break;
            default:
                // TODO: execute the action
                randAction.exec(RandomlyChoosePeople(), RandomlyChoosePeople(),
                    new Location(Random.Integer(100), Random.Integer(100)),
                    Simulator.CurrentTime);
                break;
        }
    }

}
