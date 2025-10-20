using ExprenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Application.DTO.ExpenseItemDTO;

public class CreateExpenseItemDTO
{
    public int ExpenseId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}