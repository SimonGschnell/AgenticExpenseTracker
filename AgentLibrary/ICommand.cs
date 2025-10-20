namespace AgentLibrary;

public interface ICommand
{
    public Task<string> Execute(ToolCallContext toolCallContext);
}