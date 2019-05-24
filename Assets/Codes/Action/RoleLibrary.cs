using System;
using System.Collections.Generic;

public static class RoleLibrary
{
    private static readonly Dictionary<string, RoleTypeBase> roleDict = new Dictionary<string, RoleTypeBase>
    {
        {
            // Role heard does not really even need the collection or filter as it will always be filled
            // by the talk action triggering it... These can just be commented out later
            "RoleHeard", new RoleType<Person>()
            {
                Name = "RoleHeard",
                Collection = PersonTown.Singleton.aliveResidents,
                Filter = (p, l) => p == null
            }
        },
        {
            "RoleBioMother", new RoleBioMother()
        }
    };

    public static RoleTypeBase GetRoleByName(string roleName)
    {
        return roleDict[roleName];
    }
}