using System;
using System.Collections.Generic;

public class RoleCEO : RoleType<Person>
{
    public new string Name = "RoleCEO";
    public new List<Person> Collection = PersonTown.Singleton.aliveResidents;

    public new Func<Person, List<RoleBase>, bool> Filter => CEOFilter;

    public bool CEOFilter(Person p, List<RoleBase> role_list)
    {
        // TODO: find people who is capable to found an institution
        return true;
    }
}