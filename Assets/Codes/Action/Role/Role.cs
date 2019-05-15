public class Role
{
    public string name;
    public object binding;
    // public AttributeTypes Attributes;
    // public List<T> Collection { get; }

    public Role(string name, object binding)
    {
        this.name = name;
        this.binding = binding;
    }
}