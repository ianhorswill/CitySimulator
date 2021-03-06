using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents an instance of an action having taken place
/// </summary>
public class Action
{
    /// <summary>
    /// Name of the action that occurred.
    /// </summary>
    readonly string actionName;
    /// <summary>
    /// Time within the simulated world at which the action occurred
    /// </summary>
    readonly DateTime time;
    /// <summary>
    /// Role bindings (arguments) of the action
    /// </summary>
    public readonly List<RoleBase> roles;

    /// <summary>
    /// Initializes a new instance of the Action class.
    /// </summary>
    /// <param name="actionName">Action name</param>
    /// <param name="time">Time action is being done</param>
    /// <param name="roles">Role bindings</param>
    public Action(string actionName, DateTime time, List<RoleBase> roles)
    {
        this.actionName = actionName;
        this.time = time;
        this.roles = roles;
    }

    /// <summary>
    /// Returns a string that represents the current Action.
    /// </summary>
    /// <returns>A string of the form:
    /// "actionName [at time] : roleName = object, roleName = object"
    /// </returns>
    public override string ToString()
    {
        String res = actionName + " [at " + time.ToString(Simulator.DateTimeFormat) + "] : ";
        foreach (var role in roles)
        {
            res += role.Name + " = " + role.GetBindingUntyped() + ", ";
        }
        return res.Substring(0, res.Length - 2);
    }

    /// <summary>
    /// Returns the value of the role with the specified name
    /// </summary>
    /// <param name="roleName">Name of the role</param>
    /// <returns>Value to which the role is bound</returns>
    public object this[string roleName]
    {
        get
        {
            return roles.First(rolebase => rolebase.Name.Equals(roleName)).GetBindingUntyped();
        }
    }
    
}
