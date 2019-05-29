using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Codes.Institution;

public class RoleConstructionCompany : RoleType<ConstructionCompany>
{
    public new string Name = "RoleConstructionCompany";
    public new List<ConstructionCompany> Collection = InstitutionManager.constructionCompanyList;
    public new Func<ConstructionCompany, List<RoleBase>, bool> Filter => ConstructFilter;

    public bool ConstructFilter(ConstructionCompany i, List<RoleBase> role_list)
    {
//        if (i.type.Equals("ConstructionCompany"))
//        {
//            return true;
//        }
        return true;
    }
}