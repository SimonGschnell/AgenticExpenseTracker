
using ExpenseTracker.LLMRuntime;
using ExpenseTracker.LLMRuntime.Ollama;

namespace AgentLibrary.LLMRuntime.Ollama;

public class OllamaChatLog : ILLMChatLog
{
    private string Model { get; set; }
    private readonly bool _stream;

    public OllamaChatLog(string model, bool stream)
    {
        Model = model;
        _stream = stream;
    }
    
    private readonly List<OllamaMessage> _chatLog = [];
    public IEnumerable<ILLM_Message> ChatLog => _chatLog;
    private readonly List<OllamaTool> _tools = [];
    public IEnumerable<ILLMTool> Tools => _tools;
    
    public void AddMessage(string message)
    {
        var ollamaMessage = new OllamaMessage {
            Content = message,
            Role = OllamaRoles.user
        };
        _chatLog.Add(ollamaMessage);
    }
    
    public void AddSystemMessage(string message)
    {
        var ollamaMessage = new OllamaMessage {
            Content = message,
            Role = OllamaRoles.system
        };
        _chatLog.Add(ollamaMessage);
    }

    public void AddToolResponse(string message)
    {
        var ollamaMessage = new OllamaMessage {
            Content = message,
            Role = OllamaRoles.tool
        };
        _chatLog.Add(ollamaMessage);
    }

    public void AddTool(ILLMTool tool)
    {
        if(tool is not OllamaTool ollamaTool)
            throw new ArgumentException("Tool must be of type OllamaTool");
        _tools.Add(ollamaTool);
    }

    public OllamaRequest createLlmRequest()
    {
        return new OllamaRequest {
            Model = Model,
            Stream = _stream,
            Messages = _chatLog,
            Tools = _tools
        };
    }

    public void AddAssistantMessage(ILLMRunTimeResponse reponse)
    {
        var ollamaMessage = new OllamaMessage {
            Role = OllamaRoles.assistant,
            Content = reponse.Response,
            ToolCalls = reponse.ToolCalls
        };
        _chatLog.Add(ollamaMessage);
    }

    public void AddMessageWithImage(string message, string base64Image)
    {
        var ollamaMessage = new OllamaMessage {
            Content = message,
            Role = OllamaRoles.user,
            Images = [base64Image]
        };
        _chatLog.Add(ollamaMessage);
    }

    public void SetModel(string model)
    {
        Model = model;
    }
}