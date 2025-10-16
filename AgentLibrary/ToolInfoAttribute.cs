namespace AgentLibrary;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ToolInfoAttribute : Attribute
{
    public readonly string _name;
    public readonly string _description;

    public ToolInfoAttribute(string name, string description)
    {
        _name = name;
        _description = description;
    }
}