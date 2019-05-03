// Actual Classes
public class ActionGiveBirth : ActionType
{
    public int priority = 3;

    public override string actionName => "GiveBirth";

    public override bool prerequisites(object agent, object patient, Location location, Time time)
    {
        // TODO: check the prereqs of this specific action 
        return true;
    }
    
    public override void modifications(object agent, object patient, Location location, Time time)
    {
        // TODO: modify the world
        Person[] otherPersons = new[] {(Person) patient};
        Person baby = Person.createChild((Person) agent, otherPersons);
    }
    
    public override void triggers(object agent, object patient, Location location, Time time)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}