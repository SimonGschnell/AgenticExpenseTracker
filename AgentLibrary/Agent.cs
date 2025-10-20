using System.Reflection;
using AgentLibrary.ImageReader;
using AgentLibrary.LLMRuntime;
using ExpenseTracker.LLMRuntime;
using Microsoft.Extensions.DependencyInjection;
using UnitTests;

namespace AgentLibrary;

public class Agent
{

    public Agent(ILLMAbstractFactory factory)
    {
        _factory = factory;
        ChatLog = _factory.CreateLLMRunTimeChatLog("gpt-oss:120b-cloud", false);
        _runtime = _factory.CreateLLMRuntime();
        _models = _factory.CreateLLMModels();
    }
    
    public Agent(ILLMAbstractFactory factory, IServiceScopeFactory scopeFactory) : this(factory)
    {
        _scopeFactory = scopeFactory;
    }
    
    private readonly ILLMAbstractFactory _factory;
    private readonly IServiceScopeFactory _scopeFactory;
    private IImageReader ImageReader { get; set; } = new FileStreamImageReader();
    protected readonly ILLMChatLog ChatLog;
    private readonly ILLMRuntime _runtime;
    private readonly ILLMModel _models;

    private Dictionary<ToolInformation, Type> Tools { get; set; } = [];

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
        var toolCallCount = response.GetToolCalls().Count;
        while (toolCallCount > 0)
        {
            response = await ExecuteTools(response.GetToolCalls());
            toolCallCount = response.GetToolCalls().Count;
            ChatLog.AddAssistantMessage(response);
        }

        return response.Response;
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
            if (_scopeFactory is null)
            {
                var entry = Tools.FirstOrDefault(t => t.Key.Name == toolCall.Name);
                var command = Activator.CreateInstance(entry.Value) as ICommand;
                var commandResponse = await command?.Execute(new ToolCallContext(toolCall.Arguments))!;
                ChatLog.AddToolResponse(commandResponse);
            }
            else
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var entry = Tools.FirstOrDefault(t => t.Key.Name == toolCall.Name);
                    var command = scope.ServiceProvider.GetRequiredService(entry.Value) as ICommand;
                    var commandResponse = await command?.Execute(new ToolCallContext(toolCall.Arguments))!;
                    ChatLog.AddToolResponse(commandResponse);
                }
            }
        }

        return await _runtime.SendPrompt(ChatLog);
    }

    public void AddTool(ICommand tool)
    {
        Type toolType =tool.GetType();
        var executeMethod = toolType.GetMethod("Execute");
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
        Tools.Add(toolInformation, toolType);
        ChatLog.AddTool(_factory.CreateLLMRunTimeTool(toolInformation.Name, toolInformation.Description, myParams));
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