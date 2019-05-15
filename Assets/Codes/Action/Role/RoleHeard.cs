using System.Collections.Generic;
using System.Linq;

public class RoleHeard : RoleType<Person>
{
    public override string Name => "RoleHeard";
    public override AttributeTypes Attributes => AttributeTypes.None;
    public override List<Person> Collection => ActionStatics.aliveResidents;

    public override bool Filter(Person self, object opt_initializer = null)
    {
        return true;
    }
}