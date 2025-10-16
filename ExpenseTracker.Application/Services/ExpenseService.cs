using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExprenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Services;

public class ExpenseService: IExpenseService
{
    private IExpenseRepository _expenseRepository;
    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }
    public async Task AddExpenseAsync(CreateExpenseDTO dto)
    {
        var expense = new Expense(description: dto.Description, amount: dto.Amount, category: dto.Category,
            currency: dto.Currency);
        await _expenseRepository.AddExpenseAsync(expense);
    }
}