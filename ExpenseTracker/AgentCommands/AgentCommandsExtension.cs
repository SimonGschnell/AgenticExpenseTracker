using AgentLibrary;

namespace ExpenseTracker;

public static class AgentCommandsExtension
{
    public static void AddAgentCommands(this IServiceCollection collection)
    {
        collection.AddScoped<ICommand, CreateExpenseCommand>();
        collection.AddScoped<CreateExpenseCommand>();
        collection.AddScoped<ICommand, CreateExpenseItemCommand>();
        collection.AddScoped<CreateExpenseItemCommand>();
    }
}