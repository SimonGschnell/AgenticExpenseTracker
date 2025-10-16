namespace AgentLibrary;

public interface ICommand
{
    public string Execute(ToolCallContext toolCallContext);
}