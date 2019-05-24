using System;
using System.Collections.Generic;

// EXAMPLE CODE ONLY

    // This is onlt needed if you have a very complex role that require more than
    // a name, a group to choose from, and a filter to select by.

public class RoleHeard : RoleType<Person>
{
    public new string Name = "RoleHeard";
    public new List<Person> Collection => PersonTown.Singleton.aliveResidents;

    public new Func<Person, List<RoleBase>, bool> Filter => HeardFilter;

    public bool HeardFilter(Person p, List<RoleBase> role_list)
    {
       
        return true;
    }
}