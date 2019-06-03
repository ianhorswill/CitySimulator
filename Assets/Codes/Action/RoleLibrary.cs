using System.Collections.Generic;
using Codes.Institution;

/// <summary>
/// Role library, dictionary of roles for easy lookup
/// </summary>
public static class RoleLibrary
{
    /// <summary>
    /// The roles which actions will use for role filling and filtering
    /// </summary>
    public static readonly Dictionary<string, RoleTypeBase> Roles = new Dictionary<string, RoleTypeBase>
    {
        // Uses constructor with default filter of true and ther default collection for the type
        { "Institution", new RoleType<Institution>("Institution") },
        { "Mingler", new RoleType<Person>("Mingler") },
        { "Dead", new RoleType<Person>("Dead") },
        { "ConstructionCompany", new RoleType<ConstructionCompany>("ConstructionCompany") },

        // Uses constructor with default collection but custom filter
        { "Mother", new RoleType<Person>("Mother", (person, action) =>
            person.isFemale() && person.age >= 18 && person.age <= 50 &&
            person.sigOther != null && person.sigOther.age >= 18 && person.readyForNextChild())
        },
        { "CEO", new RoleType<Person>("CEO", (person, action) =>
            person.individualPersonality.facets["STRESS_VULNERABILITY"] < 40 &&
            person.individualPersonality.facets["CONFIDENCE"] > 60
        )},
        {"Employee", new RoleType<Person>("Employee", (person, action) =>
            {
                Institution ins = (Institution) action["Institution"];
                return person.individualPersonality.facets["DUTIFULNESS"] > 70 &&
                       person.personalEducation.is_college_graduate &&
                       !ins.employeeList.Contains(person);
            }
        )},
        { "MinglingWith", new RoleType<Person>("MinglingWith", (person, action) =>
            (Person) action["Mingler"] != person ) },

        // Uses constructor with binder, thus the action input to the lambda
        { "SameLocation", new RoleType<Plot>("SameLocation", action =>
            {
                var speaker = (Person) action["Speaker"];
                var listener = (Person) action["Listener"];
                if (speaker.currentLocation == listener.currentLocation)
                    return speaker.currentLocation;
                return null;
            })
        },
        { "Father", new RoleType<Person>("Father", action => {
                var mother = (Person) action["Mother"];
                return mother.sigOther;
            })
        }
    };
}
