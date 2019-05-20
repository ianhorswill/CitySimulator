using System;
using System.Collections.Generic;
using System.Linq;

public abstract class ActionType
{
    // Required fields:
    public abstract string ActionName { get; }
    public abstract List<RoleTypeBase> Role_list { get; }

    // Optional fields (else use defaults):
    public virtual int Priority { get { return 10; } }
    public virtual double Chance { get { return 1.0; } }

    // Optional methods (else do nothing):
    public virtual void Modifications(Action a) { return; }
    public virtual void Triggers(Action a) { return; }

    public Action AttemptAction()
    {
        List<RoleBase> filled_roles = new List<RoleBase>();

        // Check every role in list and try to fill it
        foreach (var role in Role_list)
        {
            RoleBase temp = role.GetRoleUntyped(filled_roles);
            // Fill if possible, otherwise return null
            if (temp != null) { filled_roles.Add(temp); }
            else { return null; }
        }
        return new Action(this.ActionName, Simulator.CurrentTime, filled_roles);
    }
    
    public void Execute(Action a)
    {
        Modifications(a);
        Triggers(a);
    }

    public void AttemptExecute()
    {
        Action a = AttemptAction();
        if (a == null)
        {
            Execute(a);
        }
    }
}