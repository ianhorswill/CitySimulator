using System;
using System.Collections.Generic;
using Codes.Institution;

public class RoleInstitution : RoleType<Institution>
{
    public new string Name = "RoleInstitution";
    public new List<Institution> Collection = InstitutionManager.institutionList;

    public new Func<Institution, List<RoleBase>, bool> Filter => InstitutionFilter;

    public bool InstitutionFilter(Institution i, List<RoleBase> role_list)
    {
        return true;
    }
}