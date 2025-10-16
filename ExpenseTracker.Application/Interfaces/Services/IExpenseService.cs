using ExpenseTracker.Application.DTO.ExpenseDTO;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IExpenseService
{
    public Task AddExpenseAsync(CreateExpenseDTO dto);
}