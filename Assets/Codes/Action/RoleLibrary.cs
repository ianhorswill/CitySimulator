using System;
using System.Collections.Generic;

public class RoleLibrary
{
    private static readonly Dictionary<string, RoleTypeBase> roleDict = new Dictionary<string, RoleTypeBase>
    {
        {"RoleHeard", new RoleHeard() }
    };

    internal RoleTypeBase GetRoleByName(string roleName)
    {
        return roleDict[roleName];
    }
}