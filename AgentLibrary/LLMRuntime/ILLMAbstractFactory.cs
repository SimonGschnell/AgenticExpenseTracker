using AgentLibrary.LLMRuntime;
using ExpenseTracker.LLMRuntime.Ollama;
using UnitTests;

namespace ExpenseTracker.LLMRuntime;

public interface ILLMAbstractFactory
{
    ILLMRuntime CreateLLMRuntime();
    ILLMChatLog CreateLLMRunTimeChatLog(string model, bool stream);
    ILLMTool CreateLLMRunTimeTool(string name, string description, List<ToolParameter> parameters=null);
    ILLMModel CreateLLMModels();
}