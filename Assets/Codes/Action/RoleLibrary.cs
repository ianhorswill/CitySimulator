using System;
using System.Collections.Generic;
using Codes.Institution;

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
            "RoleCEO", new RoleType<Person>()
            {
                Name = "RoleCEO",
                Collection = PersonTown.Singleton.aliveResidents,
                Filter = (p, l) =>  { return true; }
            }
        },
        {
            "RoleConstructionCompany", new RoleType<ConstructionCompany>()
            {
                Name = "RoleConstructionCompany",
                Collection = InstitutionManager.constructionCompanyList,
                Filter = (p, l) => { return true; }
            }
        },
        {
            "RoleEmployee", new RoleType<Person>()
            {
                Name = "RoleEmployee",
                Collection = PersonTown.Singleton.aliveResidents,
                Filter = (p, l) => { return true; }
            }
        },
        {
            "RoleInstitution", new RoleType<Institution>()
            {
                Name = "RoleInstitution",
                Collection = InstitutionManager.institutionList,
                Filter = (p, l) =>  { return true; }
            }
        },
    };

    public static RoleTypeBase GetRoleByName(string roleName)
    {
        return roleDict[roleName];
    }
}