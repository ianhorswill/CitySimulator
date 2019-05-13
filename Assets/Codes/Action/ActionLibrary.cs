using System.Collections.Generic;
using System.Linq;
using Codes.Action.Actions;
using UnityEngine;
using Random = System.Random;

public class ActionLibrary : MonoBehaviour
{
    private static Dictionary<string, ActionType> actionDict = new Dictionary<string, ActionType>();

    private void Start()
    {
        actionDict.Add("Talk", new ActionTalk());
        actionDict.Add("Heard", new ActionHeard());
        actionDict.Add("GenerateInstitution", new ActionGenerateInstitution());
        actionDict.Add("GiveBirth", new ActionGiveBirth());
        actionDict.Add("InstitutionHiring", new ActionInstitutionHiring());
        actionDict.Add("Death", new ActionDeath());
    }

    public static ActionType RandomlyChoose()
    {
        if (actionDict.Count == 0)
        {
            return null;
        }
        // TODO: Change this over to the seed based random number generator
        Random rand = new Random();
        List<ActionType> values = Enumerable.ToList(actionDict.Values);
        int randNum = rand.Next(actionDict.Count);
        return values[randNum];
    }
    
    public static ActionType GetActionByName(string actionName)
    {
        return actionDict[actionName];
    }
}