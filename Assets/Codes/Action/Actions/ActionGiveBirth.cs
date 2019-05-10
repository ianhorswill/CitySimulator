// Actual Classes

using UnityEngine;

public class ActionGiveBirth : ActionType
{
    public override string actionName => "GiveBirth";

    public override bool prerequisites(object agent, object patient, Location location, Time time)
    {
        // TODO: check the prereqs of this specific action 
        return ActionStatics.randomNumberGenerator.Next(100) < ActionStatics.BIRTH_PROBABILITY &&
               ((Person) agent).age >= ActionStatics.MIN_AGE && ((Person) patient).age >= ActionStatics.MIN_AGE &&
               (((Person) agent).isFemale() && ((Person) patient).isMale() ||
                (((Person) agent).isMale() && ((Person) patient).isFemale()));
    }

    public override void modifications(object agent, object patient, Location location, Time time)
    {
        // TODO: modify the world
        Person[] otherPersons = {(Person) patient};
        Person baby = Person.createChild((Person) agent, otherPersons);
        ActionStatics.aliveResidents.Add(baby);
    }

    public override void triggers(object agent, object patient, Location location, Time time)
    {
        // TODO: call all the actions that will be triggered by this action
    }
}