using AgentLibrary;
using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.DTO.ExpenseItemDTO;
using ExpenseTracker.Application.Interfaces.Services;
using ExprenseTracker.Domain.ValueObjects;
using UnitTests;

namespace ExpenseTracker;

public class CreateExpenseItemCommand: ICommand
{
    private readonly IExpenseService _expenseService;

    public CreateExpenseItemCommand(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    [ToolInfo("CreateExpenseItem", "A tool to create an expenseItem connected to an expense in the expenseTracker application")]
    [ToolParameter("expenseId", "string", "The aggregate expense id to which we want to add the expenseItem")]
    [ToolParameter("amount", "string", "The amount spent for the single expenseItem")]
    [ToolParameter("name", "string", "The name of the expenseItem")]
    public async Task<string> Execute(ToolCallContext toolCallContext)
    {
        try
        {
            var expenseId = Convert.ToInt32(toolCallContext.GetArgument("expenseId"));
            var amount = Convert.ToDecimal(toolCallContext.GetArgument("amount"));
            var description = toolCallContext.GetArgument("name");
            await _expenseService.AddExpenseItem(new CreateExpenseItemDTO()
            {
                ExpenseId = expenseId,
                Amount = amount,
                Description = description
            });
            return "ExpenseItem was successfully created";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}