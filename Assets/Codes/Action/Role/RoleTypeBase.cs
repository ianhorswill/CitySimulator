using System.Collections.Generic;

/// <summary>
/// Base class for all RoleTypes
/// Represents a role, independent of the value it's bound to in a particular action.
/// RoleTypes contain the information needed to choose objects to fill the role.
/// </summary>
public abstract class RoleTypeBase
{
    /// <summary>
    /// Name of the role
    /// </summary>
    public string Name;

    /// <summary>
    /// True if the simulator should construct a new object to fill the role.
    /// </summary>
    public bool BuildFlag;

    // Allows us to get filled roles from the base class. RoleBase is used to
    // hold the various types of bindings in filled roles which is needed so that
    // each RoleType can access all previously filled roles in its Filtering...
    //  - RoleTypeBase allows for lists of roles to fill (in ActionTypes)
    //  - RoleBase allows for lists of filled roles (in RoleTypes and Actions)
    
    /// <summary>
    /// Find a value to bind this role to for some particular action.
    /// </summary>
    /// <param name="roleBindings">The existing role bindings for the action</param>
    /// <returns></returns>
    public abstract RoleBase FillRoleUntyped(List<RoleBase> roleBindings = null);

    /// <summary>
    /// Attempt to bind this role to a specific value for some action
    /// </summary>
    /// <param name="desiredValue">Value to bind it to</param>
    /// <param name="roleBindings">Existing bindings for the action</param>
    /// <returns>A binding (RoleBase) if successful, else null.</returns>
    public abstract RoleBase FillRoleWith(object desiredValue, List<RoleBase> roleBindings = null);
}