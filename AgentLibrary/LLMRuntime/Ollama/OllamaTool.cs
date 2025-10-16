using System.Text.Json.Serialization;
using UnitTests;

namespace ExpenseTracker.LLMRuntime.Ollama;

public class OllamaTool : ILLMTool
{
    private ILLMTool _illmToolImplementation;

    public OllamaTool(string name, string description, List<ToolParameter> parameters)
    {
        if (parameters is not null)
        {
            Function.Parameters = new ToolParameters();
            foreach (var parameter in parameters)
            {
                Function.Parameters.Properties.Add(parameter.Name, new ToolProperty{ Type = parameter.Type, Description = parameter.Description, Enum = parameter.Enum});
            }
        }
        Function.Name = name;
        Function.Description = description;
    }

    [JsonPropertyName("type")] public string Type { get; set; } = "function";

    [JsonPropertyName("function")] public ToolFunction Function { get; set; } = new ToolFunction();

}


public class ToolFunction
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("parameters")] 
    public ToolParameters Parameters { get; set; }
}

public class ToolParameters
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "object";

    [JsonPropertyName("properties")]
    public Dictionary<string, ToolProperty> Properties { get; set; } = new();

    [JsonPropertyName("required")]
    public List<string> Required { get; set; } = new();
}

public class ToolProperty
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("enum")]
    public List<string> Enum { get; set; } = new();
}