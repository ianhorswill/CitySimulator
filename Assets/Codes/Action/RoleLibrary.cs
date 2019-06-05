using System.Collections.Generic;

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
            person.IsFemale && person.age >= 18 && person.age <= 50 &&
            person.sigOther != null && person.sigOther.age >= 18 && person.readyForNextChild())
        },
        { "Marry", new RoleType<Person>("Marry", (person, action) =>
            person.age >= 16 && (person.sigOther == null || person.sigOther.dead))
        },
        { "Divorce", new RoleType<Person>("Divorce", (person, action) =>
            person.sigOther != null)
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
        {"FiredEmployee", new RoleType<Person>("FiredEmployee", (person, action) =>
        {
            {
                Institution ins = (Institution) action["Institution"];
                // TODO: choose the employee to be fire
                var population = PersonTown.Singleton.aliveResidents.Count;
                // below the cut job threshold
                var cut = ins.visitCount < ins.CUT_JOB_THRESHOLD * population;
                return cut && ins.employeeList.Contains(person);
            }
        })},
        {
            "Robber", new RoleType<Person>("Robber", (person, action) =>
            {
                var ins = ((Institution) action["Institution"]);
                return ins.security_level < ins.ROB_THRESHOLD && person.individualPersonality.facets["VIOLENT"] > 80;
            }
        )},
        {
            "InstitutionToIncreaseSecurity", new RoleType<Institution>("InstitutionToIncreaseSecurity", 
            (institution, action) => institution.employeeList.Count > 20 && institution.visitCount > 40)
        },
        {"VisitingPerson", new RoleType<Person>("VisitingPerson", (person, action) =>
            person.age > 7)
        },
        { "Father", new RoleType<Person>("Father", action => {
                var mother = (Person) action["Mother"];
                return mother.sigOther;
            })
        },
        { "DivorceWith", new RoleType<Person>("DivorceWith", action => {
                var partner = (Person) action["Divorce"];
                return partner.sigOther;
            })
        },
        { "MarryWith", new RoleType<Person>("MarryWith", action =>
            {
                var Bride = (Person) action["Marry"];
                foreach (Person candidate in Bride.romanticallyInterestedIn)
                {
                    if (Bride.CanMarry(candidate) && candidate.romanticallyInterestedIn.Contains(Bride))
                    {
                        return candidate;
                    }
                }
                return null;
            })
        }
    };
}
