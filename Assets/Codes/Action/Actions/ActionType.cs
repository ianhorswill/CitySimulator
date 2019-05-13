using System.Collections.Generic;

public abstract class ActionType
{
    public abstract string actionName { get;}
    public abstract int priority { get; }
    public abstract double chance { get; }
    public abstract List<RoleTypeBase> role_list { get; }

    public abstract void modifications(Action a);
    public abstract void triggers(Action a);

    public Action attemptAction()
    {
        // TODO: Change this over to the seed based random number generator
        Random rand = new Random();

        List<Role> filled_roles = new List<Role>();
        
        // Check for init role...
        var init = from i in role_list
                   where i.tag == "init"
                   select i;
        var init = init.ToList();
        if (!init.Any())
        {
            foreach (var role in role_list)
            {
                // TODO: fill each role in list
            }
        }
        // If there is an init role...
        else
        {
            var i = from i in init[0].list
                    where init[0].attribute(i)
                    select i;
            var i = i.ToList();
            if (!i.Any())
            {
                return null; // nothing could fill it
            }
            else
            {
                Role init_filled = new Role(init[0].name, i[rand.Next(i.Length)]);
                filled_roles.Add(init_filled);

                // Check for recipient role...
                var recipient = from r in role_list
                                where r.tag == "init"
                                select r;
                var recipient = recipient.ToList();
                if (!recipient.Any())
                {
                    // TODO: fill remaining roles in list
                }
                // If there is a recipient...
                else
                {
                    var r = from r in recipient[0].list
                            where recipient[0].attribute(r, i)
                            select r;
                    var r = r.ToList();
                    if (!r.Any())
                    {
                        return null; // nothing could fill it
                    }
                    else
                    {
                        Role recipient_filled = new Role(recipient[0].name, r[rand.Next(r.Length)]);
                        filled_roles.Add(recipient_filled);
                        // TODO: fill remaining roles in list
                    }
                }
            }
        }
        Action a = new Action(this.actionName, ActionStatics.t, filled_roles);
        return a;
    }
    
    public void execute(Action a)
    {
        modifications(a);
        triggers(a);
    }

    public void attemptExecute()
    {
        execute(attemptAction());
    }
}