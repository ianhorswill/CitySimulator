using System.Collections.Generic;
using System.Linq;
using Codes.Action.Actions;

public class RoleLibrary
{
    private static Dictionary<string, RoleTypeBase> roleDict = new Dictionary<string, RoleTypeBase>();

    private void Start()
    {
        roleDict.Add("Heard", new RoleHeard());
    }

    public static ActionType RandomlyChoose()
    {
        if (roleDict.Count == 0)
        {
            return null;
        }
        // TODO: Change this over to the seed based random number generator
        Random rand = new Random();
        List<RoleTypeBase> values = Enumerable.ToList(roleDict.Values);
        int randNum = rand.Next(roleDict.Count);
        return values[randNum];
    }
    
    public static ActionType GetActionByName(string actionName)
    {
        return roleDict[actionName];
    }
}