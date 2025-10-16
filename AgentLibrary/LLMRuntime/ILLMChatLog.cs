using ExpenseTracker.LLMRuntime;
using ExpenseTracker.LLMRuntime.Ollama;

namespace AgentLibrary.LLMRuntime;

public interface ILLMChatLog
{
    public IEnumerable<ILLM_Message> ChatLog { get; }
    public IEnumerable<ILLMTool> Tools { get; }
    void AddMessage(string message);
    void AddToolResponse(string message);
    void AddTool(ILLMTool tool);
    OllamaRequest createLlmRequest();
    void AddAssistantMessage(ILLMRunTimeResponse reponse);
    void AddSystemMessage(string message);
    void AddMessageWithImage(string input, string base64Image);
    
    void SetModel(string model);
}

public interface ILLM_Message
{
    string Content { get; }
}


