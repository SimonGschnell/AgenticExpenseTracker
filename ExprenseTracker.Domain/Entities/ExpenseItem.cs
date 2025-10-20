using ExprenseTracker.Domain.ValueObjects;

namespace ExprenseTracker.Domain.Entities;

public class ExpenseItem
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }
    public Money Price { get; private set; }
    private DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    private ExpenseItem()
    {
    }

    public ExpenseItem(string description, Category category, Money price)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (price.Amount <= 0)
            throw new InvalidOperationException("Price must be greater than zero.");

        Description = description;
        Price = price;
        Category = category;
    }
    
    //navigation
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; }
}