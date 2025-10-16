using AgentLibrary;

namespace UnitTests;

public class RumpelstilzchenCommand : ICommand
{
    [ToolInfo("GuessMyName", "A tool to guess the name of the user.")]
    public string Execute(ToolCallContext toolCallContext)
    {
        return "My name is Rumpelstilzchen.";
    }
}