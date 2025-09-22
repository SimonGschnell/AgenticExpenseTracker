using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExprenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IExpenseRepository
{
    public Task AddExpenseAsync(Expense expense);
}