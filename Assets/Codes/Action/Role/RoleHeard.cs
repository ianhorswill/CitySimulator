using System.Collections.Generic;
using System.Linq;

public class RoleHeard : RoleType<Person>
{
    public override string name => "RoleHeard";
    public override string tag => "";
    public override List<Person> list => ActionStatics.aliveResidents;

    public override bool attribute(object self, params object[] args)
    {
        // TODO: person that isn't deaf
        // TODO: filter by location
        return true;
    }
}