using System;
using System.Collections.Generic;
using System.Linq;

public abstract class ActionType
{
    public abstract string ActionName { get;}
    public abstract int Priority { get; }
    public abstract double Chance { get; }
    public abstract List<RoleTypeBase> Role_list { get; }

    public abstract void Modifications(Action a);
    public abstract void Triggers(Action a);

    private RoleTypeBase GetRoleTypeByAttribute(AttributeTypes Attributes)
    {
        IEnumerable<RoleTypeBase> temp = from r in this.Role_list
                                         where r.Attributes == Attributes
                                         select r;
        List<RoleTypeBase> tempList = temp.ToList();
        // We only allow ONE role of each AttributeType per list (other than None type)
        if (tempList.Count > 0) { return tempList[0]; }
        else { return null; }
    }

    private List<Role> RoleFiller(List<Role> filled_roles)
    {
        // Look for a build object...
        RoleTypeBase build = GetRoleTypeByAttribute(AttributeTypes.Build);
        if (build != null)
        {
            // Create dummy...
            // TODO: 'smart' object creation based on underlying type?
            Role buildRole = new Role(build.Name, new object());
            filled_roles.Add(buildRole);
        }
        foreach (var role in Role_list)
        {
            if (role.Attributes == AttributeTypes.None)
            {
                Role temp = role.GetRole();
                if (temp != null) { filled_roles.Add(temp); }
                else { return null; }
            }
        }
        return filled_roles;
    }

    public Action AttemptAction()
    {
        List<Role> filled_roles = new List<Role>();
        // Start with the initiator...
        RoleTypeBase initiator = GetRoleTypeByAttribute(AttributeTypes.Initiator);
        if (initiator != null)
        {
            // Fill in the init if we have one...
            Role initiatorRole = initiator.GetRole();
            if (initiatorRole == null) { return null; }
            else
            {
                filled_roles.Add(initiatorRole);
                // Check for a recipient
                RoleTypeBase recipient = GetRoleTypeByAttribute(AttributeTypes.Recipient);
                if (recipient != null)
                {
                    // Fill in the recipient...
                    Role recipientRole = recipient.GetRole(initiatorRole.binding);
                    if (recipientRole == null) { return null; }
                    else
                    {
                        filled_roles.Add(recipientRole);
                        // In any case, check the remaining roles...
                        filled_roles = RoleFiller(filled_roles);
                    }
                }
                else { filled_roles = RoleFiller(filled_roles); }
            }
        }
        else { filled_roles = RoleFiller(filled_roles); }
        // Final check of if all roles were filled...
        if (filled_roles != null)
        {
            return new Action(this.ActionName, ActionStatics.t, filled_roles);
        }
        else { return null; }
    }
    
    public void Execute(Action a)
    {
        Modifications(a);
        Triggers(a);
    }

    public void AttemptExecute()
    {
        Execute(AttemptAction());
    }
}