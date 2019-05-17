using System;
using System.Collections.Generic;

public class Action
{
    readonly string actionName;
    // TODO: Change to framework time datatype
    readonly Time time;
    readonly List<RoleBase> roles;

    public Action(string actionName, Time time, List<RoleBase> roles)
    {
        this.actionName = actionName;
        this.time = time;
        this.roles = roles;
    }
    
    public override string ToString()
    {
        String res = actionName + " [at " + time.timestamp + "] : ";
        foreach (var role in roles)
        {
            // TODO: better string then printing out the bound object
            res += role.Name + " = " + role.GetBindingUntyped() + ", ";
        }
        return res.Substring(0, res.Length - 2);
    }
}