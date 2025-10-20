using AgentLibrary;

namespace UnitTests;

public class RumpelstilzchenCommand : ICommand
{
    [ToolInfo("GuessMyName", "A tool to guess the name of the user.")]
    public Task<string> Execute(ToolCallContext toolCallContext)
    {
        return Task.FromResult("My name is Rumpelstilzchen.");
    }
}