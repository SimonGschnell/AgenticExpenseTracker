using System.Reflection;
using AgentLibrary.ImageReader;
using AgentLibrary.LLMRuntime;
using ExpenseTracker.LLMRuntime;
using UnitTests;

namespace AgentLibrary;

public class Agent(ILLMAbstractFactory factory)
{
    private IImageReader ImageReader { get; set; } = new FileStreamImageReader();
    protected readonly ILLMChatLog ChatLog = factory.CreateLLMRunTimeChatLog("gpt-oss:120b-cloud", false);
    private readonly ILLMRuntime _runtime = factory.CreateLLMRuntime();
    private readonly ILLMModel _models = factory.CreateLLMModels();

    private Dictionary<ToolInformation, ICommand> Tools { get; set; } = [];

    public async Task<string> SendPromptAsync(string input)
    {
        ChatLog.SetModel(_models.ModelOptimizedForText);
        ChatLog.AddMessage(input);
        return await InteractWithLlm();
    }
    
    public async Task<string> SendPromptWithImagePathAsync(string input, string path)
    {
        var base64String =ImageReader.GetBase64FromImageUrl(path);
        ChatLog.SetModel(_models.ModelOptimizedForImage);
        ChatLog.AddMessageWithImage(input, base64String);
        var result = await InteractWithLlm();
        return result;
    }
    
    public async Task<string> SendPromptWithImageAsync(string input, string base64Image)
    {
        ChatLog.SetModel(_models.ModelOptimizedForImage);
        ChatLog.AddMessageWithImage(input, base64Image);
        var result = await InteractWithLlm();
        return result;
    }

    private async Task<string> InteractWithLlm()
    {
        var response = await GetLlmResponse();
        if (response.GetToolCalls().Count == 0) return response.Response;
        
        //todo: loop until no more tools to call
        var llmToolResponse = await ExecuteTools(response.GetToolCalls());
        return llmToolResponse.Response;
    }

    private async Task<ILLMRunTimeResponse> GetLlmResponse()
    {
        ILLMRunTimeResponse response = await _runtime.SendPrompt(ChatLog);
        ChatLog.AddAssistantMessage(response);
        return response;
    }

    private async Task<ILLMRunTimeResponse> ExecuteTools(List<ILLMToolCall> toolCalls)
    {
        foreach (var toolCall in toolCalls)
        {
            var entry = Tools.FirstOrDefault(t => t.Key.Name == toolCall.Name);
            var commandResponse = entry.Value.Execute(new ToolCallContext(toolCall.Arguments));
            ChatLog.AddToolResponse(commandResponse);
        }

        return await _runtime.SendPrompt(ChatLog);
    }

    public void AddTool(ICommand tool)
    {
        Type t =tool.GetType();
        var executeMethod = t.GetMethod("Execute");
        var toolInfo = executeMethod.GetCustomAttribute(typeof(ToolInfoAttribute), false);
        var toolParameters = executeMethod.GetCustomAttributes(typeof(ToolParameter), false);
        var myTool = toolInfo as ToolInfoAttribute;
        var myParams = toolParameters.Cast<ToolParameter>().ToList();
        var toolInformation = new ToolInformation()
        {
            Name = myTool._name,
            Description = myTool._description,
            Parameters = myParams
        };
        Tools.Add(toolInformation, tool);
        ChatLog.AddTool(factory.CreateLLMRunTimeTool(toolInformation.Name, toolInformation.Description, myParams));
    }
    
    public void SetImageReader(IImageReader reader)
    {
        ImageReader = reader;
    }
    
    public IEnumerable<ILLM_Message> GetChatLog()
    {
        return ChatLog.ChatLog;
    }
}