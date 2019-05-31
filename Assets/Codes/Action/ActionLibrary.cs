using System;
using System.Collections.Generic;
using System.Linq;
using Codes.Institution;
using UnityEngine;
using static RoleLibrary;

public static class ActionLibrary
{
    private static readonly double DictionaryWeightSum = ActionDict.Skip(1).Sum(x => x.Value.Frequency);

    private static readonly SortedDictionary<string, ActionType> ActionDict = new SortedDictionary<string, ActionType>
    {
        { "Talk" , new ActionType("Talk",  GetRoleByName("RoleSpeaker"), GetRoleByName("RoleListener"), GetRoleByName("RoleSameLocation"))
            {
                Frequency = 100.0,
                Modifications = a =>
                {
                    var Listener = (Person)a["RoleListener"];
                    // TODO: influence speakers
                },
                PostExecute = a =>
                {
                    var Listener = (Person) a["RoleListener"];
                    // TODO: filter based on location, people nearby this conversation
                    Action heard = ActionLibrary.InstantiateByName("Heard", "RoleHeard", Listener);
                    if (heard != null)
                    {
                        ExecuteByName("Heard", heard);
                    }
                }
            }
        },
        { "Heard" , new ActionType("Heard", GetRoleByName("RoleHeard"))
        },
        { "GiveBirth" , new ActionType("GiveBirth", GetRoleByName("RoleBioMother"))
           {
                Frequency = 1.0,
                Modifications = a =>
                {
                    var BioMother = (Person) a["BioMother"];
                    var BioFather = BioMother.sigOther;
                    Person baby = Person.createChild(BioMother, BioFather);
                    PersonTown.Singleton.aliveResidents.Add(baby);
                }
            }
        },
        { "Death", new ActionType("Death", GetRoleByName("RoleDeath"))
            {
                Frequency = 0.0001,
                Modifications = a =>
                {
                    var selectedToDie= (Person) a["Death"];
                    selectedToDie.dead = true;
                    PersonTown.Singleton.aliveResidents.Remove(selectedToDie);
                    PersonTown.Singleton.deceased.Add(selectedToDie);
                }
            }
        },
        { "GenerateInstitution", new ActionType("GenerateInstitution", RoleLibrary.GetRoleByName("RoleCEO"), RoleLibrary.GetRoleByName("RoleConstructionCompany"))
            {
                Frequency = 0.3,
                Modifications = a =>
                {
                    Institution ins = InstitutionManager.GeneratorInstitution(
                        (Person) a["CEO"],
                        InstitutionManager.GetRandomType(),
                        Space.Singleton.get_random_plot());
        
                    ((ConstructionCompany) a["ConstructionCompany"]).Build(ins);
                }
            }
        },
        { "InstitutionHiring", new ActionType("InstitutionHiring", RoleLibrary.GetRoleByName("RoleInstitution"), RoleLibrary.GetRoleByName("RoleEmployee"))
            {
                Frequency = 0.3,
                Modifications = a =>
                {
                    ((Institution) a["Institution"]).Hiring((Person) a["Employee"]);
                }
            }
        }
    };

    public static ActionType GetActionByName(string actionName)
    {
        return ActionDict[actionName];
    }

    public static Action InstantiateByName(string name, params object[] bindings)
    {
        return GetActionByName(name).Instantiate(bindings);
    }

    public static void ExecuteByName(string name, Action act)
    {
        GetActionByName(name).Execute(act);
    }

    public static ActionType RandomlyChoose()
    {
        return RandomlyChoose(ActionDict);
    }

    private static ActionType RandomlyChoose(SortedDictionary<string, ActionType> actionSubset)
    {
        if (actionSubset.Count != 0)
        {
            List<ActionType> values = Enumerable.ToList(actionSubset.Values);
            return values.RandomElement();

        }
        return null;
    }

    public static ActionType ChanceFilteredRandomSelection()
    {
        float rand = Random.Float(0.0f, (float)DictionaryWeightSum);
        float sum = 0.0f;

        foreach (KeyValuePair<string, ActionType> entry in ActionDict)
        {
            sum += (float)entry.Value.Frequency;
            if (sum > rand)
            {
                return entry.Value;
            }
        }
        return null;
    }
}