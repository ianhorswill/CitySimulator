using System;
using System.Collections.Generic;
using System.Linq;
using Codes.Institution;
using static RoleLibrary;

/// <summary>
/// Action library, stores all Actions and helps randomly select an action.
/// </summary>
public static class ActionLibrary
{
    /// <summary>
    /// The action dictionary (sorted so that access - looping - is ordered)
    /// </summary>
    private static readonly SortedDictionary<string, ActionType> ActionDict = new SortedDictionary<string, ActionType>
    {
        // EXAMPLE CODE:
        //{ "Talk" , new ActionType("Talk", Roles["Speaker"], Roles["Listener"], Roles["SameLocation"])
        //    {
        //        PostExecute = a =>
        //        {
        //            var Listener = (Person) a["Listener"];
        //            GetActionByName("Heard").InstantiateAndExecute("Hearer", Listener);
        //        }
        //    }
        //},
        //{ "Heard" , new ActionType("Heard", Roles["Hearer"]) },

        // TODO: assimilate the location filter logic into the Mingle Action
        // TODO: Add heard action to PostExecute of Mingle for gossip

        // REAL ACTION LISTINGS:
        { "Mingle" , new ActionType("Mingle", Roles["Mingler"], Roles["MinglingWith"])
            {
                Modifications = a =>
                {
                    var MinglingWith = (Person)a["MinglingWith"];
                    var Mingler = (Person)a["Mingler"];
                    var compat = Person.Relationship.getCompatibility(MinglingWith, Mingler)/100.0;

                    int sparkBaseRate = 2;
                    int sparkChange = (int) Math.Ceiling((double) (sparkBaseRate*compat));

                    int chargeBaseRate = 2;
                    int chargeChange = (int) Math.Floor( (double) (chargeBaseRate*compat));

                    MinglingWith.updateRelationshipSpark(Mingler, sparkChange);
                    MinglingWith.updateRelationshipCharge(Mingler, chargeChange);
                    Mingler.updateRelationshipSpark(MinglingWith, sparkChange);
                    Mingler.updateRelationshipCharge(MinglingWith, chargeChange);

                    Mingler.getCaptivatedIndividuals();
                    Mingler.getRomanticInterests();
                    MinglingWith.getCaptivatedIndividuals();
                    MinglingWith.getRomanticInterests();

                }
            }
        },
        { "Birth" , new ActionType("Birth", Roles["Mother"])
           {
                Frequency = 0.1f,
                Modifications = a =>
                {
                    var Mother = (Person) a["Mother"];
                    // TODO: Add role for father that filters based on being the significant other
                    var Father = Mother.sigOther;
                    // TODO: Use build role to create new child
                    // TODO: add Build<Action, object> function and RegisterBuilder(Person, Build)
                    Person baby = Person.createChild(Mother, Father);
                    PersonTown.Singleton.aliveResidents.Add(baby);
                }
            }
        },
        { "Death", new ActionType("Death", Roles["Dead"])
            {
                Frequency = 0.0001f,
                Modifications = a =>
                {
                    var selectedToDie = (Person)a["Dead"];
                    selectedToDie.dead = true;
                    PersonTown.Singleton.aliveResidents.Remove(selectedToDie);
                    PersonTown.Singleton.deceased.Add(selectedToDie);
                }
            }
        },
        { "GenerateInstitution", new ActionType("GenerateInstitution", Roles["CEO"], Roles["ConstructionCompany"])
            {
                Frequency = 0.3f,
                Modifications = a =>
                {
                    // TODO: Use build role to create an institution
                    // TODO: add Build<Action, object> function and RegisterBuilder(Institution, Build)
                    Institution ins = InstitutionManager.GeneratorInstitution(
                        (Person) a["CEO"],
                        InstitutionManager.GetRandomType(),
                        Space.Singleton.get_random_plot());

                    // Generating an institution also has the construction company build it.
                    ((ConstructionCompany) a["ConstructionCompany"]).Build(ins);
                }
            }
        },
        { "InstitutionHiring", new ActionType("InstitutionHiring", Roles["Institution"], Roles["Employee"])
            {
                Frequency = 0.3f,
                Modifications = a =>
                {
                    ((Institution) a["Institution"]).Hiring((Person) a["Employee"]);
                }
            }
        }
    };

    /// <summary>
    /// Gets the action by name. Used for PostModifications to get specific actions
    /// </summary>
    /// <returns>The action named</returns>
    /// <param name="actionName">Action name.</param>
    public static ActionType GetActionByName(string actionName)
    {
        return ActionDict[actionName];
    }

    /// <summary>
    /// The reference action, calibrates the frequencies of actions
    /// </summary>
    private static readonly ActionType ReferenceAction = ActionDict["Mingle"];
    /// <summary>
    /// The target number of iterations for the reference action to acheive
    /// </summary>
    private static readonly float TargetReferenceInterations = 1.0f;
    /// <summary>
    /// Sum of all action frequencies
    /// </summary>
    private static readonly float FrequencyWeightSum = ActionDict.Skip(1).Sum(x => x.Value.Frequency);
    /// <summary>
    /// The number of actual iterations (number of iterations to get ReferenceAction
    /// to happen TargetReferenceInterations times), based on the sum of all
    /// action frequencies...
    /// </summary>
    public static readonly float ActualIterations = TargetReferenceInterations * (FrequencyWeightSum / ReferenceAction.Frequency);

    /// <summary>
    /// Selects an action at random following the frequencies provided for actions
    /// </summary>
    /// <returns>The filtered random action selection.</returns>
    public static ActionType FrequencyFilteredRandomSelection()
    {
        float rand = Random.Float(0.0f, FrequencyWeightSum);
        float sum = 0.0f;

        // Using the sum of all frequencies, get a random number in that range and
        // keep iterating through the library until the frequency sum surpasses the
        // random number selected.
        foreach (KeyValuePair<string, ActionType> entry in ActionDict)
        {
            sum += entry.Value.Frequency;
            if (sum > rand) { return entry.Value; }
        }
        return null;
    }
}