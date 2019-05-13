using System.Collections.Generic;
using System.Linq;

public class RoleHeard : RoleType<Person>
{
    public override string name => "RoleHeard";
    public override string tag => "";
    public override List<Person> list => ActionStatics.aliveResidents;

    public override bool attribute(Person self, params object[] args)
    {
        // TODO: filter by location
        return true;
    }
}