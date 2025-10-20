using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Infrastructure.Context;
using ExprenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

public class ExpenseItemRepository: IExpenseItemRepository
{

    private AppDbContext _context;
    public ExpenseItemRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ExpenseItem>> GetAll()
    {
        return await _context.ExpenseItems.ToListAsync();
    }

    public async Task<ExpenseItem> GetById(int id)
    {
        return await _context.ExpenseItems.FirstAsync(e => e.Id == id);
    }
}