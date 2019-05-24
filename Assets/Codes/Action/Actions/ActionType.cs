using System.Collections.Generic;

/// <summary>
/// Represents a kind of action, as opposed to an instance of an action
/// </summary>
public abstract class ActionType
{
    // Required fields:

    public abstract string ActionName { get; }

    /// <summary>
    /// List of rules that need to be bound in an instance of this action type
    /// </summary>
    public abstract List<RoleTypeBase> Role_list { get; }

    // Optional fields (else use defaults):
    public virtual int Priority { get { return 10; } }
    public virtual double Chance { get { return 1.0; } }

    // Optional methods (else do nothing):

    /// <summary>
    /// Change the world to reflect the execution of the specified action
    /// </summary>
    /// <param name="a">The action to execute.  Will always be an instance of this action type</param>
    public virtual void Modifications(Action a) { return; }
    /// <summary>
    /// Perform any further operations associated with the execution of an action, after the world is
    /// modified.  For example, queuing subsequent actions to perform in the future.
    /// </summary>
    /// <param name="a">Original action being executed</param>
    public virtual void PostExecute(Action a) { return; }

    // roleBindings expects alternating strings (naming a role) and objects (to fill that role)

    /// <summary>
    /// Create an instance of this action type by finding values to fill each of its roles
    /// </summary>
    /// <param name="roleBindings">List of bindings to force for the actions roles</param>
    /// <returns></returns>
    public Action Instantiate(params object[] roleBindings)
    {
        
        List<RoleBase> filled_roles = new List<RoleBase>();

        foreach (var role in Role_list)
        {
            var temp_binding_obj = BindingOf(role.Name, roleBindings);
            if (temp_binding_obj != null)
            {
                // AGAIN, NOT TYPE SAFE, RELIES ON PROPER TYPE BEING PASSED IN
                RoleBase temp = role.FillRoleWith(temp_binding_obj, filled_roles);
                if (temp != null) { filled_roles.Add(temp); }
                else { return null; }
            }
            else
            {
                RoleBase temp = role.FillRoleUntyped(filled_roles);
                if (temp != null) { filled_roles.Add(temp); }
                else { return null; }
            }
        }
        return new Action(this.ActionName, Simulator.CurrentTime, filled_roles);
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
        Modifications(a);
        PostExecute(a);
    }
}