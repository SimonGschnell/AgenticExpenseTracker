using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Infrastructure.Context;
using ExprenseTracker.Domain.Entities;

namespace ExpenseTracker.Infrastructure.Repositories;

public class ExpenseRepository: IExpenseRepository
{

    private AppDbContext _context;
    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddExpenseAsync(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
    }
}