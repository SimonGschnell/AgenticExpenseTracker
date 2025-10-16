using UnitTests;

namespace AgentLibrary;

public class ToolInformation
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ToolParameter> Parameters { get; set; }
}