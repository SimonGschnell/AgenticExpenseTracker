using AgentLibrary.LLMRuntime;
using AgentLibrary.LLMRuntime.Ollama;
using UnitTests;

namespace ExpenseTracker.LLMRuntime.Ollama;

public class OllamaAbstractFactory: ILLMAbstractFactory
{
    public ILLMRuntime CreateLLMRuntime()
    {
        return OllamaRuntime.Create("http://localhost:11434");
    }

    public ILLMChatLog CreateLLMRunTimeChatLog(string model, bool stream)
    {
        return new OllamaChatLog(model, stream);
    }

    public ILLMTool CreateLLMRunTimeTool(string name, string description, List<ToolParameter> parameters)
    {
        return new OllamaTool(name, description, parameters);
    }

    public ILLMModel CreateLLMModels()
    {
        return new OllamaModels();
    }
}