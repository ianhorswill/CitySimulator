/// <summary>
/// A Role is a filled in role (not a type of role with logic about filtering).
/// Represents a binding of a role to a value. This is the Type specific version
/// for type checking that gets passed to the untyped base.
/// </summary>
/// <typeparam name="T">Type of the role's value</typeparam>
public class Role<T> : RoleBase
{
    /// <summary>
    /// The value this Role is bound to
    /// </summary>
    public readonly T value;

    /// <summary>
    /// Make a binding of the specified name to the specified value
    /// </summary>
    /// <param name="name">name of the role</param>
    /// <param name="value">value of the role</param>
    public Role(string name, T value)
    {
        Name = name;
        this.value = value;
    }

    /// <summary>
    /// Get the value to which this role is bound
    /// </summary>
    /// <returns>value of the role</returns>
    public T GetBinding() { return value; }

    /// <summary>
    /// Get the value to which this role is bound, hooks up to the Base class for
    /// type agnostic filled Role code
    /// </summary>
    /// <returns>Value of the role</returns>
    public override object GetBindingUntyped() { return GetBinding(); }
}