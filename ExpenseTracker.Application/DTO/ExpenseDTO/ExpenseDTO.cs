using ExprenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Application.DTO.ExpenseDTO;

public class ExpenseDTO
{
    public  string Description { get; set; }
    public Money Price { get; set; }
    public  Category Category { get; set; }
}