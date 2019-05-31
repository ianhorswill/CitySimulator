using System.Collections.Generic;
using Codes.Institution;
using UnityEditor.PackageManager;
using UnityEditor.UIElements;
using UnityEngine;

public static class RoleLibrary
{
    private static readonly Dictionary<string, RoleTypeBase> RoleDict = new Dictionary<string, RoleTypeBase>
    {
        { "RoleSpeaker", new RoleType<Person>("Speaker") },
        { "RoleListener", new RoleType<Person>("Listener") },
        { "RoleHeard", new RoleType<Person>("Heard") },
        { "RoleBioMother", new RoleType<Person>("BioMother", (p, bindings) => 
            p.isFemale() && p.age >= 18 && p.age <= 50 && p.sigOther != null && p.sigOther.age >= 18 && p.readyForNextChild()) },
        { "RoleDeath", new RoleType<Person>("Death") },
        { "RoleSameLocation", new RoleType<Plot>("Location", a =>
                                                                        {
                                                                            var speaker = (Person) a["Speaker"];
                                                                            var listener = (Person) a["Listener"];
                                                                            if (speaker.currentLocation == listener.currentLocation)
                                                                                return speaker.currentLocation;
                                                                            return null;
                                                                        })
        },
        { "RoleCEO", new RoleType<Person>("CEO", (person, action) => 
//            person.age > 15 &&  
            person.individualPersonality.facets["STRESS_VULNERABILITY"] < 40 &&
            person.individualPersonality.facets["CONFIDENCE"] > 60
        )},
        { "RoleConstructionCompany", new RoleType<ConstructionCompany>("ConstructionCompany", InstitutionManager.constructionCompanyList) },
        {
            "RoleEmployee", new RoleType<Person>("Employee", (e, bindings) =>
                {
                    Institution ins = (Institution) bindings["Institution"];
                    return e.individualPersonality.facets["DUTIFULNESS"] > 70 && 
                           e.personalEducation.is_college_graduate &&
                           !ins.employeeList.Contains(e);
                }
        )},
        { "RoleInstitution", new RoleType<Institution>("Institution")}
    };

    public static RoleTypeBase GetRoleByName(string roleName)
    {
        return RoleDict[roleName];
    }
}
