using System;
using System.Collections.Generic;

public static class RoleLibrary
{
    private static readonly Dictionary<string, RoleTypeBase> roleDict = new Dictionary<string, RoleTypeBase>
    {
        { "RoleHeard", new RoleType<Person>("RoleHeard") },
        { "RoleBioMother", new RoleType<Person>("RoleBioMother", (p, bindings) => p.isFemale() && p.age >= 18 && p.sigOther != null && p.sigOther.age >= 1) }
    };

    public static RoleTypeBase GetRoleByName(string roleName)
    {
        return roleDict[roleName];
    }
}