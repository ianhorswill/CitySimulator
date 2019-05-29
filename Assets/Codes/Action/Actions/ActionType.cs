using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a kind of action, as opposed to an instance of an action
/// </summary>
public class ActionType
{
    /// <summary>
    /// Create a new object describing a new type of action with the specified roles.
    /// </summary>
    /// <param name="name">Name of the type of action</param>
    /// <param name="roles">Roles (arguments) of actions of this type.</param>
    public ActionType(string name, params RoleTypeBase[] roles)
    {
        ActionName = name;
        RoleList = new List<RoleTypeBase>(roles);
    }

    /// <summary>
    /// Name of this type of action
    /// </summary>
    public string ActionName;

    /// <summary>
    /// List of rules that need to be bound in an instance of this action type
    /// </summary>
    public List<RoleTypeBase> RoleList;

    // Optional fields (else use defaults):
    public int Priority = 10;
    public double Chance = 1;

    // Optional methods (else do nothing):

    /// <summary>
    /// Change the world to reflect the execution of the specified action
    /// </summary>
    public Action<Action> Modifications;

    /// <summary>
    /// Perform any further operations associated with the execution of an action, after the world is
    /// modified.  For example, queuing subsequent actions to perform in the future.
    /// </summary>
    public Action<Action> PostExecute;

    // roleBindings expects alternating strings (naming a role) and objects (to fill that role)

    /// <summary>
    /// Create an instance of this action type by finding values to fill each of its roles
    /// </summary>
    /// <param name="roleBindings">List of bindings to force for the actions roles</param>
    /// <returns></returns>
    public Action Instantiate(params object[] roleBindings)
    {
        
        var filledRoles = new List<RoleBase>();
        var a = new Action(ActionName, Simulator.CurrentTime, filledRoles);
        
        foreach (var role in RoleList)
        {
            var tempBindingObj = BindingOf(role.Name, roleBindings);
            if (tempBindingObj != null)
            {
                // AGAIN, NOT TYPE SAFE, RELIES ON PROPER TYPE BEING PASSED IN
                RoleBase temp = role.FillRoleWith(tempBindingObj, a);
                //Debug.Log("fill");
                if (temp != null) { filledRoles.Add(temp); }
                else { return null; }
            }
            else
            {
                RoleBase temp = role.FillRoleUntyped(a);
                //Debug.Log("fill untyped");
                if (temp != null) { filledRoles.Add(temp); }
                else { return null; }
            }
        }
        return a;
    }

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
    /// <param name="a">Action to perform</param>
    public void Execute(Action a)
    {
        // TODO: log action in global action list
        //ActionSimulator.action_history.Add(a);
        if (Modifications != null)
            Modifications(a);
        if (PostExecute != null)
            PostExecute(a);
    }
}