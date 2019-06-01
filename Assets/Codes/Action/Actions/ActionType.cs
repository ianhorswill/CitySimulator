using System;
using System.Collections.Generic;

/// <summary>
/// Represents a kind of action, as opposed to an instance of an action
/// </summary>
public class ActionType
{
    /// <summary>
    /// Name of this type of action
    /// </summary>
    public string ActionName;
    /// <summary>
    /// The frequency at which this ActionType should occur, 1.0 is the default.
    /// Higher for more frequent, Lower for less frequent. Exact probability is
    /// managed by the sum of all frequencies, balanced against a reference ActionType
    /// for a specified number of expected occurances (see ActionLibrary)
    /// </summary>
    public float Frequency = 1;
    /// <summary>
    /// List of roles that need to be bound in an instance of this action type
    /// </summary>
    public List<RoleTypeBase> RoleList;

    /// <summary>
    /// Change the world to reflect the execution of the specified action
    /// </summary>
    public Action<Action> Modifications;
    /// <summary>
    /// Perform any further operations associated with the execution of an action,
    /// after the world is modified. For example, queuing subsequent actions to
    /// perform in the future.
    /// </summary>
    public Action<Action> PostExecute;
    // TODO: add ability to queue an action for later steps.

    /// <summary>
    /// Create a new ActionType describing a new type of action with the specified roles.
    /// </summary>
    /// <param name="name">Name of the type of action</param>
    /// <param name="role_list">Roles (arguments) of actions of this type.</param>
    public ActionType(string name, params RoleTypeBase[] role_list)
    {
        ActionName = name;
        RoleList = new List<RoleTypeBase>(role_list);
    }

    /// <summary>
    /// Create an instance of this action type by finding values to fill each of
    /// its roles. Roles can be filled by passing in a string (with the role name)
    /// and an object (to fill that role) which will be validated by the filter
    /// method. This is not type safe, it relies on proper type being passed in.
    /// Or, the role will be attempted to be filled by the regular Role filling function.
    /// </summary>
    /// <param name="roleBindings">List of bindings to force for the actions roles,
    ///   expects alternating strings (naming a role) and objects (to fill that role)</param>
    /// <returns></returns>
    public Action Instantiate(params object[] roleBindings)
    {
        List<RoleBase> filledRoles = new List<RoleBase>();
        Action action = new Action(ActionName, Simulator.CurrentTime, filledRoles);
        
        foreach (var role in RoleList)
        {
            var BoundObject = BindingOf(role.Name, roleBindings);
            if (BoundObject != null)
            {
                RoleBase temp = role.FillRoleWith(BoundObject, action);
                if (temp != null) { filledRoles.Add(temp); }
                else { return null; }
            }
            else
            {
                RoleBase temp = role.FillRoleUntyped(action);
                if (temp != null) { filledRoles.Add(temp); }
                else { return null; }
            }
        }
        return action;
    }

    /// <summary>
    /// Helps us treat the object[] roleBindings from Instantiate as a dictionary
    /// </summary>
    /// <returns>The object to be bound to the role</returns>
    /// <param name="role">string specifying the binding object to use</param>
    /// <param name="bindings">Bindings, passed along from Instantiate</param>
    object BindingOf(string role, object[] bindings)
    {
        for (int i = 0; i < bindings.Length - 1; i += 2)
            if ((string)bindings[i] == role)
                return bindings[i + 1];
        return null;
    }
    
    /// <summary>
    /// Perform the action in the (simulated) world
    /// </summary>
    /// <param name="action">Action to perform</param>
    public void Execute(Action action)
    {
        ActionSimulator.action_history.Add(action);
        Modifications?.Invoke(action);
        PostExecute?.Invoke(action);
    }

    /// <summary>
    /// Instantiates and (if the action was instatiated) executes it.
    /// </summary>
    /// <param name="bindings">Bindings to pass to Instantiate</param>
    public void InstantiateAndExecute(params object[] bindings)
    {
        Action act = Instantiate(bindings);
        if (act != null) { Execute(act); }
    }
}