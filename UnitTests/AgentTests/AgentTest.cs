using AgentLibrary;
using AgentLibrary.ImageReader;
using ExpenseTracker.LLMRuntime;
using ExpenseTracker.LLMRuntime.Ollama;

namespace UnitTests;

public class AgentTest
{
    private class MockAgent(ILLMAbstractFactory factory) : Agent(factory)
    {
        public IEnumerable<ILLMTool> GetChatlogTools()
        {
            return ChatLog.Tools;
        }
    }
    
    [Fact]
    public async Task RequestResponseWithLlm()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        var agent = new Agent(factory);
        ICommand tool = new RumpelstilzchenCommand();
        agent.AddTool(tool);
        IUser user = new User(agent);
        
        var response = await user.Prompt("What is the capital of France? Answer in one word.");
        
        Assert.Contains("Paris".ToLower(), response.ToLower());
    }
    
    [Fact]
    public async Task MultipleRequestsToLlm()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        var agent = new Agent(factory);
        IUser user = new User(agent);
        
        var response = await user.Prompt("What is the capital of France? Answer in one word.");
        var response2 = await user.Prompt("What is the capital of Italy? Answer in one word.");
        
        Assert.Contains("Paris".ToLower(), response.ToLower());
        Assert.Equal("Rome".ToLower(), response2.ToLower());
    }
    
    [Fact]
    public async Task RequestWithTool()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        var agent = new Agent(factory);
        IUser user = new User(agent);
        ICommand tool = new RumpelstilzchenCommand();
        agent.AddTool(tool);
        
        var response = await user.Prompt("Use the tool GuessMyName to find out my name.");
        
        Assert.Contains("Rumpelstilzchen".ToLower(), response.ToLower());
    }
    
    [Fact]
    public async Task RequestWithToolThatHasAnArgument()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        var agent = new Agent(factory);
        IUser user = new User(agent);
        ICommand tool = new GetWeatherCommand(new WeatherStation());
        agent.AddTool(tool);
        
        var response = await user.Prompt("Get the weather in Berlin using the tool GetWeather.");
        
        Assert.Contains("25", response.ToLower());
    }
    
    [Fact]
    public async Task LlmCallsMultipleTools()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        var agent = new Agent(factory);
        IUser user = new User(agent);
        ICommand tool1 = new RumpelstilzchenCommand();
        ICommand tool2 = new GetWeatherCommand(new WeatherStation());
        agent.AddTool(tool1);
        agent.AddTool(tool2);
        
        var response = await user.Prompt("Use the tool GuessMyName to find out my name and get the weather in Berlin using the tool GetWeather.");
        
        Assert.Contains("Rumpelstilzchen".ToLower(), response.ToLower());
        Assert.Contains("25", response.ToLower());
        Assert.Equal(6,agent.GetChatLog().Count());
        Assert.Single(agent.GetChatLog().Where(message => message is OllamaMessage{Role: OllamaRoles.user}));
        Assert.Equal(3,agent.GetChatLog().Count(message => message is OllamaMessage{Role: OllamaRoles.assistant}));
        Assert.Equal(2,agent.GetChatLog().Count(message => message is OllamaMessage{Role: OllamaRoles.tool}));
    }
    
    [Fact]
    public Task VerifyToolCountInChatLog()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        var agent = new MockAgent(factory);
        IUser user = new User(agent);
        ICommand tool = new RumpelstilzchenCommand();
        agent.AddTool(tool);
        
        user.Prompt("what is the weather in Berlin?");
        user.Prompt("what is the weather in Rome?");
            
        Assert.Single(agent.GetChatlogTools());
        return Task.CompletedTask;
        // Code to pretty print the chatlog of the agent:
        // var messages = _agent.GetChatLog();
        // var jsonSerializationOptions = new JsonSerializerOptions() { WriteIndented = true };
        // jsonSerializationOptions.Converters.Add(new JsonStringEnumConverter());
        // var serializedMessages = JsonSerializer.Serialize(messages,messages.GetType(), jsonSerializationOptions);
        // return serializedMessages;
    }
    
    [Fact]
    public async Task PromptWithImage()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        IImageReader imageReader = new FileStreamImageReader();
        var agent = new Agent(factory);
        agent.SetImageReader(imageReader);
        IUser user = new User(agent);
        
        var response = await user.PromptWithImage(
                "is there a person in the image? answer with yes or no.",
                Path.Combine(AppContext.BaseDirectory, "profil.jpg")
            );
            
        Assert.Contains("yes".ToLower(), response.ToLower());
    }
    
    
}