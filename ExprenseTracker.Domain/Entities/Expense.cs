using ExprenseTracker.Domain.ValueObjects;

namespace ExprenseTracker.Domain.Entities;

public class Expense
{
    public int Id { get; private set; }
    public  string Description { get; private set; }
    public Money Price { get; private set; }
    public  Category Category { get; private set; }

    private DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    private DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    private Expense()
    {
    }

    public Expense(string description, Category? category, decimal amount, Currency currency)
    {
        Price = new Money(){ Amount = amount, Currency = currency };
        Description = description;
        Category = category ?? Category.Other;
    }

    public void UpdatePrice(decimal? amount, Currency? currency)
    {
        Price = Price with { Amount = amount ?? Price.Amount, Currency = currency ?? Price.Currency };   
        UpdatedAt = DateTime.UtcNow;
        
    }
    
    public void UpdateCategory(Category category)
    {
        Category = category;
        UpdatedAt = DateTime.UtcNow;
        
    }
    
    public void UpdateDescription(string description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow;
        
    }
    
    public void AddItem(ExpenseItem expenseItem)
    {
        ExpenseItems.Add(expenseItem);
        expenseItem.Expense = this;
    }
    
    //navigation
    public List<ExpenseItem> ExpenseItems { get; set; } = [];

}