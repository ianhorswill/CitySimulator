
public class ActionDeath : ActionType
{
    public override string actionName => "Death";

    public override bool prerequisites(object agent, object patient, Location location, Time time)
    {
        // TODO: check the prereqs of this specific action 
        return Random.Integer(100) < ActionStatics.DEATH_PROBABILITY;
    }
    
    public override void modifications(object agent, object patient, Location location, Time time)
    {
        // TODO: modify the world
        ActionStatics.aliveResidents.Remove((Person) agent);
        ActionStatics.deceased.Add((Person) agent);
    }
    
    public override void triggers(object agent, object patient, Location location, Time time)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}