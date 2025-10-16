using ExpenseTracker.LLMRuntime.Ollama;

namespace ExpenseTracker.LLMRuntime;

public interface ILLMRunTimeResponse
{
    public string Response { get; }
    public string Thinkinig { get; }
    
    public List<ToolCall> ToolCalls { get; }

    public List<ILLMToolCall> GetToolCalls();
}