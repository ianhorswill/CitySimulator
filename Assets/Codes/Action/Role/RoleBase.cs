public abstract class RoleBase
{
    public string Name { get; set; }

    public abstract object GetBindingUntyped();
}