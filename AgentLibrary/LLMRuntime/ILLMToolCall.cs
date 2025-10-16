namespace ExpenseTracker.LLMRuntime;

public interface ILLMToolCall
{
    public string Name { get; set; }
    public Dictionary<string, string> Arguments { get; set; }
}