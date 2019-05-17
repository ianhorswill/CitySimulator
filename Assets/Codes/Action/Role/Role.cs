public class Role<T> : RoleBase
{
    public T binding;

    public Role(string name, T binding)
    {
        this.Name = name;
        this.binding = binding;
    }

    public T GetBinding()
    {
        return binding;
    }

    public override object GetBindingUntyped()
    {
        return GetBinding();
    }
}