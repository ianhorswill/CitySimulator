using System.Collections.Generic;
using System.Linq;
using Codes.Action.Actions;
using UnityEngine;
using Random = System.Random;

// TODO: Switch to simulation thread framework
public class ActionLibrary : MonoBehaviour
{
    private static readonly Dictionary<string, ActionType> actionDict = new Dictionary<string, ActionType>();
    public static readonly RoleLibrary roleLibrary = new RoleLibrary();

    private void Start()
    {
        actionDict.Add("Talk", new ActionTalk());
        actionDict.Add("Heard", new ActionHeard());
        actionDict.Add("GenerateInstitution", new ActionGenerateInstitution());
        actionDict.Add("GiveBirth", new ActionGiveBirth());
        actionDict.Add("InstitutionHiring", new ActionInstitutionHiring());
        actionDict.Add("Death", new ActionDeath());
    }

    public static ActionType GetActionByName(string actionName)
    {
        return actionDict[actionName];
    }

    public static ActionType ChanceFilteredPriorityBasedSelection(int priority)
    {
        ActionType a = PriorityBasedSelection(priority);
        // TODO: change 1.0 to be a random value from 0 to 1
        if (1.0 - a.Chance < 0.0)
        {
            return null;
        }
        else
        {
            return a;
        }
    }

    public static ActionType PriorityBasedSelection(int priority)
    {
        var result = from a in actionDict
                     where a.Value.Priority == priority
                     select a;
        return RandomlyChoose(result.ToDictionary(t => t.Key, t => t.Value));
    }

    private static ActionType RandomlyChoose(Dictionary<string, ActionType> actionSubset)
    {
        if (actionSubset.Count == 0)
        {
            return null;
        }
        else
        {
            // TODO: replace with framework random
            Random rnd = new Random();
            List<ActionType> values = Enumerable.ToList(actionSubset.Values);
            return values[rnd.Next(actionSubset.Count)];
        }
    }

    public static ActionType RandomlyChoose()
    {
        return RandomlyChoose(actionDict);
    }
}