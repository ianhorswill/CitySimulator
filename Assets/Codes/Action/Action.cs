
public class Action
{
    string ActionName;
    object agent;
    object patient;
    Location location;
    Time time;
}

abstract class ActionDefinition
{
    public int priority = 0;
    abstract public bool prerequisites();
    abstract public void triggers();
}

public class ActionTalk : ActionDefinition
{
    public override bool prerequisites()
    {
        // TODO: do something
    }
    
    public override void triggers()
    {
        // TODO: do something
    }
}

