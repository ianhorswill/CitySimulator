using System.Collections.Generic;

public static class RoleLibrary
{
    private static readonly Dictionary<string, RoleTypeBase> RoleDict = new Dictionary<string, RoleTypeBase>
    {
        { "RoleSpeaker", new RoleType<Person>("Speaker") },
        { "RoleListener", new RoleType<Person>("Listener") },
        { "RoleHeard", new RoleType<Person>("Heard") },
        { "RoleBioMother", new RoleType<Person>("BioMother", (p, bindings) => p.isFemale() && p.age >= 18 && p.sigOther != null && p.sigOther.age >= 1) },
        { "RoleSameLocation", new RoleType<Plot>("Location", a =>
                                                                        {
                                                                            var speaker = (Person) a["Speaker"];
                                                                            var listener = (Person) a["Listener"];
                                                                            if (speaker.currentLocation == listener.currentLocation)
                                                                                return speaker.currentLocation;
                                                                            return null;
                                                                        })
        }
    };

    public static RoleTypeBase GetRoleByName(string roleName)
    {
        return RoleDict[roleName];
    }
}
