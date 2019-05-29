using System;
using System.Collections.Generic;
using System.Linq;
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
                    var BioMother = ((Role<Person>) a["BioMother"]).value;
                    var BioFather = BioMother.sigOther;
                    Person baby = Person.createChild(BioMother, BioFather);
                    PersonTown.Singleton.aliveResidents.Add(baby);
                }
            }
        },

        { "Death", new ActionType("Death", GetRoleByName("RoleDeath"))
            {
                Priority = 3,
                Chance = 0.001,
                Modifications = a =>
                {
                    var selectedToDie= ((Role<Person>) a["Death"]).value;
                    selectedToDie.dead = true;
                    PersonTown.Singleton.aliveResidents.Remove(selectedToDie);
                    PersonTown.Singleton.deceased.Add(selectedToDie);
                }
            }
        },

        {
            "Mingle", new ActionType(name: "Mingle", GetRoleByName("RoleSpeaker"), GetRoleByName("RoleListener"))
            {
                Priority = 2,
                Chance = 1.0,
                Modifications = a =>
                {
                    var p1 = ((Role<Person>) a["Listener"]).value;
                    var p2 = ((Role<Person>) a["Speaker"]).value;


                    var compat = Person.Relationship.getCompatibility(p2, p1)/100 - 0.5;


                    int sparkBaseRate = 5;
                    int sparkChange = (int) Math.Ceiling(sparkBaseRate*compat);

                    int chargeBaseRate = 10;
                    int chargeChange = (int) Math.Floor(chargeBaseRate*compat);

                    p2.updateRelationshipSpark(p1, sparkChange);
                    p2.updateRelationshipCharge(p1, chargeChange);
                    p1.updateRelationshipSpark(p2, sparkChange);
                    p1.updateRelationshipCharge(p2, chargeChange);

                }
            }

        }

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