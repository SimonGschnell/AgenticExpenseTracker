using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Infrastructure.Context;
using ExprenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
    }

    public async Task<Expense> GetById(int id)
    {
        return await _context.Expenses
            .Include(e=> e.ExpenseItems)
            .FirstAsync(e => e.Id == id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}