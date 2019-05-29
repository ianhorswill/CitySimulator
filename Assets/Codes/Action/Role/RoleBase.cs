/// <summary>
/// Represents the binding of a role to a value
/// </summary>
public abstract class RoleBase
{
    /// <summary>
    /// Name of the role being bound
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Returns the value to which it is bound
    /// </summary>
    /// <returns>The value to which the role is bound</returns>
    public abstract object GetBindingUntyped();
}