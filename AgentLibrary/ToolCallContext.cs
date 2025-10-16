namespace AgentLibrary;

public record ToolCallContext(Dictionary<string, string> ToolCallArguments)
{
    public string GetArgument(string name)
    {
        if (ToolCallArguments.ContainsKey(name))
            return ToolCallArguments[name];
        return string.Empty;
    }
};