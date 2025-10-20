using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.DTO.ExpenseItemDTO;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExprenseTracker.Domain.Entities;
using ExprenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Application.Services;

public class ExpenseService: IExpenseService
{
    private IExpenseRepository _expenseRepository;
    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }
    public async Task<int> AddExpenseAsync(CreateExpenseDTO dto)
    {
        var expense = new Expense(description: dto.Description, amount: dto.Amount, category: dto.Category,
            currency: dto.Currency);
        await _expenseRepository.AddExpenseAsync(expense);
        return expense.Id;
    }

    public async Task<ExpenseDTO> GetById(int id)
    {
        var expense = await _expenseRepository.GetById(id);
        return new ExpenseDTO(){Description = expense.Description, Category = expense.Category, Price = expense.Price};
    }

    public async Task AddExpenseItem(CreateExpenseItemDTO dto)
    {
        var expense = await _expenseRepository.GetById(dto.ExpenseId);
        var expenseItem = new ExpenseItem(dto.Description, expense.Category, expense.Price with { Amount = dto.Amount });
        expense.AddItem(expenseItem);
        await _expenseRepository.SaveAsync();
    }
}