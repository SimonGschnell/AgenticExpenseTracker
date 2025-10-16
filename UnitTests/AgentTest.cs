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
    public async Task MultipleRequestsWithTools()
    {
        ILLMAbstractFactory factory = new OllamaAbstractFactory();
        var agent = new Agent(factory);
        IUser user = new User(agent);
        ICommand tool1 = new RumpelstilzchenCommand();
        ICommand tool2 = new GetWeatherCommand(new WeatherStation());
        agent.AddTool(tool1);
        agent.AddTool(tool2);
        
        var response1 = await user.Prompt("Use the tool GuessMyName to find out my name.");
        var response2 = await user.Prompt("Get the weather in Berlin using the tool GetWeather.");
        
        Assert.Contains("Rumpelstilzchen".ToLower(), response1.ToLower());
        Assert.Contains("25", response2.ToLower());
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