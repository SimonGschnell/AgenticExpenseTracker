using AgentLibrary;
using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Services;
using ExprenseTracker.Domain.ValueObjects;
using UnitTests;

namespace ExpenseTracker;

public class CreateExpenseCommand: ICommand
{
    private readonly IExpenseService _expenseService;

    public CreateExpenseCommand(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    [ToolInfo("CreateExpense", "A tool to create expenses in the expenseTracker application")]
    [ToolParameter("amount", "string", "The total amount spent on the receipt")]
    [ToolParameter("category", "string", "The category of the expense", typeof(Category))]
    [ToolParameter("currency", "string", "The currency of the expense", typeof(Currency))]
    [ToolParameter("description", "string", "An overall summary of the expense (short)")]
    public string Execute(ToolCallContext toolCallContext)
    {
        try
        {
            var amount = Convert.ToDecimal(toolCallContext.GetArgument("amount"));
            var category = Enum.Parse<Category>(toolCallContext.GetArgument("category"));
            var currency = Enum.Parse<Currency>(toolCallContext.GetArgument("currency"));
            var description = toolCallContext.GetArgument("description");
            _expenseService.AddExpenseAsync(new CreateExpenseDTO
                { Amount = amount, Description = description, Category = category, Currency = currency });
            return "Expense was successfully created";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}