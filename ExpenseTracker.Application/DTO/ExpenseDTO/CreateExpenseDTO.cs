using ExprenseTracker.Domain;
using ExprenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Application.DTO.ExpenseDTO;

public record CreateExpenseDTO
{
    public Category Category { get; init; }
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
}