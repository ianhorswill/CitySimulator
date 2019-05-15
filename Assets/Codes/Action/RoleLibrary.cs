using System.Collections.Generic;

public class RoleLibrary
{
    private static readonly Dictionary<string, RoleTypeBase> roleDict = new Dictionary<string, RoleTypeBase>();

    private void Build()
    {
        roleDict.Add("Heard", new RoleHeard());
    }
    
    public static RoleTypeBase GetRoleByName(string roleName)
    {
        return roleDict[roleName];
    }
}