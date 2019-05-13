using System.Collections.Generic;

public abstract class ActionType
{
    public abstract string actionName { get;}
    public abstract int priority { get; }
    public abstract double chance { get; }
    public abstract List<RoleTypeBase> role_list { get; }

    public abstract void modifications(Action a);
    public abstract void triggers(Action a);

    public Action attemptAction()
    {
        // TODO: For each roleType, try to fill it
        List<Role> filled_roles = new List<Role>();
        // TODO: If all filled, create action
        Action currAction = new Action(this.actionName, ActionStatics.t, filled_roles);
        return currAction;
    }
    
    public void execute(Action a)
    {
        modifications(a);
        triggers(a);
    }

    public void attemptExecute()
    {
        execute(attemptAction());
    }
}