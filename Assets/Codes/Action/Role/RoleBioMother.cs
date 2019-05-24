using System;
using System.Collections.Generic;

public class RoleBioMother : RoleType<Person>
{
    public new string Name = "RoleBioMother";
    public new List<Person> Collection = PersonTown.Singleton.aliveResidents;

    public new Func<Person, List<RoleBase>, bool> Filter => BioMotherFilter;

    public bool BioMotherFilter(Person p, List<RoleBase> role_list)
    {
        if (p.isFemale() && p.age >= 18 && p.sigOther != null && p.sigOther.age >= 18)
        {
            return true;
        }
        return false;
    }
}