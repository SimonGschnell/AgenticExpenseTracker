using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExprenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IExpenseItemRepository
{
    public Task<List<ExpenseItem>> GetAll();
    public Task<ExpenseItem> GetById(int id);
}