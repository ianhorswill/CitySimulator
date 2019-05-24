using System;
using System.Collections.Generic;
using UnityEngine;

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
                Filter = (p, l) =>
                {
                    return true;
                }
            }
        },
        {
            "RoleBioMother", new RoleType<Person>()
            {
                Name = "RoleBioMother",
                Collection = PersonTown.Singleton.aliveResidents,
                Filter = (p, l) =>
                {
                    if (p.isFemale() && p.age >= 18 && p.sigOther != null && p.sigOther.age >= 18)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
    };

    public static RoleTypeBase GetRoleByName(string roleName)
    {
        return roleDict[roleName];
    }
}