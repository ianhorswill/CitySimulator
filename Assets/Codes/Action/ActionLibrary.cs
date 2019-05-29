using System;
using System.Collections.Generic;
using System.Linq;
using Codes.Action.Actions;

public static class ActionLibrary
{
    private static readonly Dictionary<string, ActionType> actionDict = new Dictionary<string, ActionType>
    {
//        { "Talk" , new ActionTalk() },
//        { "Heard" , new ActionHeard() },
        { "GenerateInstitution" , new ActionGenerateInstitution() },
//        { "GiveBirth" , new ActionGiveBirth() },
//        { "InstitutionHiring" , new ActionInstitutionHiring() },
//        { "Death" , new ActionDeath() }
    };

    public static ActionType GetActionByName(string actionName)
    {
        return actionDict[actionName];
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
        return RandomlyChoose(actionDict);
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
        var result = from a in actionDict
                     where a.Value.Priority == priority
                     select a;
        // TODO: special logic based on various priority levels
        return RandomlyChoose(result.ToDictionary(t => t.Key, t => t.Value));
    }

    public static ActionType ChanceFilteredPriorityBasedSelection(int priority)
    {
        ActionType a = PriorityBasedSelection(priority);
        return a;
//        return (Random.Float(0.0f, 1.0f) - a.Chance) < 0.0 ? null : a;
    }
}