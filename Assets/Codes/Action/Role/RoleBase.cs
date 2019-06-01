/// <summary>
/// A Role is a filled in role (not a type of role with logic about filtering).
/// Represents a binding of a role to a value. This is the generic version for
/// type agnostic filled role code in actions. Allows for lists of filled roles
/// (see RoleTypes and Actions)
/// </summary>
public abstract class RoleBase
{
    /// <summary>
    /// Name of the role being bound
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Returns the value to which it is bound (links with the type specific Roles)
    /// </summary>
    /// <returns>The value to which the role is bound</returns>
    public abstract object GetBindingUntyped();
}