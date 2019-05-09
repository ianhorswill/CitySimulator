using System.Collections.Generic;
using System.Linq;
using Codes.Action.Actions;

public class RoleLibrary
{
    private static Dictionary<string, RoleType<object>> roleDict = new Dictionary<string, RoleType<object>>();

    private void Start()
    {
        roleDict.Add("Heard", new RoleHeard());
    }

    public static ActionType RandomlyChoose()
    {
        if (actionDict.Count == 0)
        {
            return null;
        }
        Random rand = new Random();
        List<ActionType> values = Enumerable.ToList(actionDict.Values);
        int size = actionDict.Count;
        int randNum = rand.Next(size);
        return values[randNum];
    }
    
    public static ActionType GetActionByName(string actionName)
    {
        return actionDict[actionName];
    }
}