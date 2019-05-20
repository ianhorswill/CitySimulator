using System;
using System.Collections.Generic;
using System.Linq;

public class RoleHeard : RoleType<Person>
{
    public override string Name => "RoleHeard";
    // TODO: switch this over to framework collections
    public override List<Person> Collection => ActionStatics.aliveResidents;

    public override Func<Person, List<RoleBase>, bool> Filter => HeardFilter;
    //public override Func<Person, List<RoleBase>, bool> Filter = (p, l) => p == null;

    public bool HeardFilter(Person p, List<RoleBase> role_list)
    {
        return true;
    }
}