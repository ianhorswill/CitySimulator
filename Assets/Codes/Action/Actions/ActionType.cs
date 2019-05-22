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
    public virtual void PostExecute(Action a) { return; }

    // roleBindings expects alternating strings (naming a role) and objects (to fill that role)
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
    
    public void Execute(Action a)
    {
        // TODO: log action in global action list
        //ActionSimulator.action_history.Add(a);
        Modifications(a);
        PostExecute(a);
    }
}