using System;
using System.Collections.Generic;
using System.Linq;

public abstract class ActionType
{
    public abstract string ActionName { get;}
    public abstract List<RoleTypeBase> Role_list { get; }

    public virtual int Priority { get { return 10; } }
    public virtual double Chance { get { return 1.0; } }

    public abstract void Modifications(Action a);
    public abstract void Triggers(Action a);

    public Action AttemptAction()
    {
        // TODO: Change this over to the seed based random number generator
        Random rand = new Random();

        List<RoleBase> filled_roles = new List<RoleBase>();

        foreach (var role in Role_list)
        {
            RoleBase temp = role.GetRoleUntyped(filled_roles);
            if (temp == null)
            {
                return null;
            }
            else
            {
                filled_roles.Add(temp);
            }
        }
        Action a = new Action(this.ActionName, ActionStatics.t, filled_roles);
        return a;
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