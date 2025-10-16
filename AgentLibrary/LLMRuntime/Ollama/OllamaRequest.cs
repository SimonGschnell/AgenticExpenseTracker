using System.Text.Json.Serialization;
using AgentLibrary.LLMRuntime;

namespace ExpenseTracker.LLMRuntime.Ollama;

public class OllamaRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<OllamaMessage> Messages { get; set; }
    
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
    
    [JsonPropertyName("tools")]
    public List<OllamaTool> Tools { get; set; }

    public OllamaRequest(string model = "gpt-oss:120b-cloud", bool stream = false)
    {
        Model = model;
        Stream = stream;
        Messages =
        [
            new OllamaMessage()
            {
                Content = "You are a helpful assistant.",
                Role = OllamaRoles.system
            }
        ];
    }
}

public record OllamaMessage : ILLM_Message
{
    [JsonPropertyName("role")]
    public OllamaRoles Role { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; init; } = string.Empty;
    [JsonPropertyName("tool_calls")]
    public List<ToolCall> ToolCalls { get; init; }

    [JsonPropertyName("images")] 
    public List<string> Images { get; init; } = [];

};

public enum OllamaRoles
{
    system,
    user,
    tool,
    assistant
}
