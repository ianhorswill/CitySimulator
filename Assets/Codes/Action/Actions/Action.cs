using System;
using System.Collections.Generic;

public class Action
{
    string actionName;
    Time time;
    List<Role> roles;

    public Action(string actionName, Time time, List<Role> roles)
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
            res += role.name + " = " + role.binding.toString() + ", ";
        }
        return res.Substring(0, res.Length - 2);
    }
}