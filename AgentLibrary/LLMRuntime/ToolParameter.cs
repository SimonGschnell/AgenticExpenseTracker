namespace UnitTests;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ToolParameter : Attribute
{
    public readonly string Name;
    public readonly string Type;
    public readonly string Description;
    public readonly List<string> Enum;

    public ToolParameter(string name, string type, string description, Type @enum=null)
    {
        Name = name;
        Type = type;
        Description = description;
        Enum = @enum is not null? System.Enum.GetNames(@enum).ToList(): null;
    }
}