namespace AgentLibrary.LLMRuntime;

public interface ILLMModel
{
    public string ModelOptimizedForText { get; }
    public string ModelOptimizedForImage { get; }
}