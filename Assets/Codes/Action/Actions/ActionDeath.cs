
using System;

public class ActionDeath : ActionType
{
    public override string actionName => "Death";

    public override bool prerequisites(object agent, object patient, Location location, DateTime time)
    {
        // TODO: check the prereqs of this specific action 
        return Random.Integer(100) < ActionStatics.DEATH_PROBABILITY;
    }
    
    public override void modifications(object agent, object patient, Location location, DateTime time)
    {
        // TODO: modify the world
        PersonTown.Singleton.aliveResidents.Remove((Person) agent);
        PersonTown.Singleton.deceased.Add((Person) agent);
    }
    
    public override void triggers(object agent, object patient, Location location, DateTime time)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}