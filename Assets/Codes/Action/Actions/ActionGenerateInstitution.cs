// Actual Classes


using System;
using System.Collections.Generic;
using Codes.Action.Actions;
using Codes.Institution;

public class ActionGenerateInstitution : ActionType
{
    public Institution Institution;

    public override string ActionName => "GenerateInstitution";

    public override double Chance => 1.0;

    public override List<RoleTypeBase> Role_list => throw new NotImplementedException();
}