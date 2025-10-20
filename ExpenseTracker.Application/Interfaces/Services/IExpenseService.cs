using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.DTO.ExpenseItemDTO;
using ExprenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IExpenseService
{
    public Task<int> AddExpenseAsync(CreateExpenseDTO dto);
    
    public Task<ExpenseDTO> GetById(int id);

    public Task AddExpenseItem(CreateExpenseItemDTO dto);
}