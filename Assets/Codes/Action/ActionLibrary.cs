using System;
using System.Collections.Generic;
using System.Linq;
using Codes.Institution;
using UnityEngine;
using static RoleLibrary;

public static class ActionLibrary
{
    private static readonly Dictionary<string, ActionType> ActionDict = new Dictionary<string, ActionType>
    {
        //{
        //    "Talk" , new ActionType("Talk",  GetRoleByName("RoleSpeaker"), GetRoleByName("RoleListener"), GetRoleByName("RoleSameLocation"))
        //        { 
        //            Priority = 5,
        //            Modifications = a =>
        //            {
        //                var Listener = (Person)a["RoleListener"];
        //                //var topic = (Person)a["RoleConverstaionTopic
        //            },

        //            PostExecute = a =>
        //            {
        //                var Listener = (Person) a["RoleListener"];
        //                // TODO?: filter based on location, people nearby this conversation
        //                Action heard = ActionLibrary.InstantiateByName("Heard", "RoleHeard", Listener);
        //                if (heard != null)
        //                {
        //                    ExecuteByName("Heard", heard);
        //                }

        //            }
        //        }
        //},



        { "Heard" , new ActionType("Heard", GetRoleByName("RoleHeard"))
        },
        //{ "GenerateInstitution" , new ActionType("GenerateInstitution") { Chance = 1 } },

        { "GiveBirth" , new ActionType("GiveBirth", GetRoleByName("RoleBioMother"))
           {
                Priority = 2,
                Chance = 1.0,
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
                Priority = 2,
                Chance = 0.0001,
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
                Priority = 2,
                Chance = 0.3,
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
                Priority = 2,
                Chance = 0.3,
                Modifications = a =>
                {
                    ((Institution) a["Institution"]).Hiring((Person) a["Employee"]);
                }
            }
        },

        { "Mingle" , new ActionType("Mingle", GetRoleByName("RoleMingler"), GetRoleByName("RoleMinglingWith"))
            {
                Priority = 2,
                Chance = 1.0,
                Modifications = a =>
                {
                    var MinglingWith = (Person)a["MinglingWith"];
                    var Mingler = (Person)a["Mingler"];
                    var compat = Person.Relationship.getCompatibility(MinglingWith, Mingler)/100;


                    int sparkBaseRate = 30;
                    int sparkChange = (int) Math.Ceiling((double) (sparkBaseRate*compat));

                    int chargeBaseRate = 30;
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

        //{ "InstitutionHiring" , new ActionType("InstitutionHiring") { Chance = 1.0 } },
        //{ "Death" , new ActionType("Death") { Chance = 1 } }
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

    private static ActionType RandomlyChoose(Dictionary<string, ActionType> actionSubset)
    {
        if (actionSubset.Count != 0)
        {
            List<ActionType> values = Enumerable.ToList(actionSubset.Values);
            return values.RandomElement();

        }
        return null;
    }

    public static ActionType PriorityBasedSelection(int priority)
    {
        var result = from a in ActionDict
                     where a.Value.Priority == priority
                     select a;
        // TODO: special logic based on various priority levels
        return RandomlyChoose(result.ToDictionary(t => t.Key, t => t.Value));
    }

    public static ActionType ChanceFilteredPriorityBasedSelection(int priority)
    {
        ActionType a = PriorityBasedSelection(priority);
        if (a == null) return null;
        return (Random.Float(0.0f, 1.0f) - a.Chance) < 0.0 ? a : null;
    }
}