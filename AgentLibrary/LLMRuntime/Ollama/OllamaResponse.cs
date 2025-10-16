using System.Text.Json.Serialization;

namespace ExpenseTracker.LLMRuntime.Ollama;

public class OllamaResponse : ILLMRunTimeResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("remote_model")]
    public string RemoteModel { get; set; }

    [JsonPropertyName("remote_host")]
    public string RemoteHost { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("message")]
    public OllamaContent Message { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }

    [JsonPropertyName("total_duration")]
    public long TotalDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public int PromptEvalCount { get; set; }

    [JsonPropertyName("eval_count")]
    public int EvalCount { get; set; }

    public string Response => Message.Content;
    public string Thinkinig => Message.Thinking;
    public List<ToolCall> ToolCalls => Message.ToolCalls;

    public List<ILLMToolCall> GetToolCalls()
    {
        var toolCalls = new List<ILLMToolCall>();
        foreach (var toolCall in Message.ToolCalls)
        {
            toolCalls.Add(new ToolResponseFunction {
                Name = toolCall.Function.Name,
                Arguments = toolCall.Function.Arguments
            });
        }

        return toolCalls;
    }
}


public class OllamaContent
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("tool_calls")] 
    public List<ToolCall> ToolCalls { get; set; } = [];
    
    [JsonPropertyName("thinking")]
    public string Thinking { get; set; }
}

public class ToolCall
{
    [JsonPropertyName("function")]
    public ToolResponseFunction Function { get; set; }
}

public class ToolResponseFunction : ILLMToolCall
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("arguments")]
    public Dictionary<string, string> Arguments { get; set; }
}


