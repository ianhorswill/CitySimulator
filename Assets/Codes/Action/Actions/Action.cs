using System;
using System.Collections.Generic;
using System.Linq;

public class Action
{
    readonly string actionName;
    readonly DateTime time;
    public readonly List<RoleBase> roles;

    public Action(string actionName, DateTime time, List<RoleBase> roles)
    {
        this.actionName = actionName;
        this.time = time;
        this.roles = roles;
    }
    
    public override string ToString()
    {
        String res = actionName + " [at " + time.ToString(Simulator.DateTimeFormat) + "] : ";
        foreach (var role in roles)
        {
            // TODO: better string then printing out the object
            res += role.Name + " = " + role.GetBindingUntyped() + ", ";
        }
        return res.Substring(0, res.Length - 2);
    }

    public object this[string roleName]
    {
        get
        {
            return roles.First(rolebase => rolebase.Name == roleName);
        }
    }
}