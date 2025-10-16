using AgentLibrary.LLMRuntime;

namespace ExpenseTracker.LLMRuntime;

public interface ILLMRuntime
{
    public Task<ILLMRunTimeResponse> SendPrompt(ILLMChatLog request);
}