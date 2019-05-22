using System;
using System.Collections.Generic;

public abstract class RoleTypeBase
{
    public string Name;
    public virtual bool BuildFlag { get { return false; } }

    // Allows us to get filled roles from the base class. RoleBase is used to
    // hold the various types of bindings in filled roles which is needed so that
    // each RoleType can access all previously filled roles in its Filtering...
    //  - RoleTypeBase allows for lists of roles to fill (in ActionTypes)
    //  - RoleBase allows for lists of filled roles (in RoleTypes and Actions)
    public abstract RoleBase FillRoleUntyped(List<RoleBase> filled_roles = null);

    // Allows us to fill roles directly in Instantiate... NOT TYPE SAFE
    public abstract RoleBase FillRoleWith(object toFill, List<RoleBase> filled_roles = null);
}