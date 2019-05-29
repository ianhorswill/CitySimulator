/// <summary>
/// Represents a binding of a role to a value
/// </summary>
/// <typeparam name="T">Type of the role's value</typeparam>
public class Role<T> : RoleBase
{
    public readonly T value;

    /// <summary>
    /// Make a binding of the role with the specified name to the specified value
    /// </summary>
    /// <param name="name">Name of the role</param>
    /// <param name="value">Value of the role</param>
    public Role(string name, T value)
    {
        Name = name;
        this.value = value;
    }

    /// <summary>
    /// Get the value to which this role is bound
    /// </summary>
    /// <returns>Value of the role</returns>
    public T GetBinding()
    {
        return value;
    }

    /// <summary>
    /// Get the value to which this role is bound
    /// </summary>
    /// <returns>Value of the role</returns>
    public override object GetBindingUntyped()
    {
        return GetBinding();
    }
}