/// <summary>
/// Base class for all RoleTypes. Allows for lists of roles to fill (see ActionTypes).
/// Represents a role, independent of the value it's bound to in a particular action.
/// Utilizes the RoleBase type to return filled in Roles.
/// </summary>
public abstract class RoleTypeBase
{
    /// <summary>
    /// Name of the role
    /// </summary>
    public string Name;

    /// <summary>
    /// Find a value to bind this role to for some particular action.
    /// </summary>
    /// <param name="action">The action being passed along to filter by
    /// the already filled items in the binding list</param>
    /// <returns></returns>
    public abstract RoleBase FillRoleUntyped(Action action);

    /// <summary>
    /// Attempt to bind (fill) this role to a specific value for some action
    /// </summary>
    /// <param name="toFill">Value to bind it to</param>
    /// <param name="action">The action being bound</param>
    /// <returns>A binding (RoleBase) if successful, else null.</returns>
    public abstract RoleBase FillRoleWith(object toFill, Action action);
}