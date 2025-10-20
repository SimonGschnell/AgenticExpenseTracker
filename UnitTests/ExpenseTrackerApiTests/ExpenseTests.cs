using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Controllers;
using ExprenseTracker.Domain.Entities;
using ExprenseTracker.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.ExpenseTrackerApiTests;

public class ExpenseTests
{
    [Fact]
    public async Task CreateExpense_CallsAddExpense()
    {
        var mockExpenseRepository = new Mock<IExpenseRepository>();
        var expenseService = new ExpenseService(mockExpenseRepository.Object);
        var createExpenseDto = new CreateExpenseDTO
        {
            Description = "Test expense",
            Amount = 50.50m,
            Category = Category.Food,
            Currency = Currency.Euro
        };
        mockExpenseRepository
            .Setup(s => s.AddExpenseAsync(It.IsAny<Expense>()))
            .Returns(Task.CompletedTask);
        
        await expenseService.AddExpenseAsync(createExpenseDto);
        
        mockExpenseRepository.Verify(r => r.AddExpenseAsync(It.IsAny<Expense>()), Times.Once);
    }
    
    [Fact]
    public Task Domain_VerifyAddItemOnExpense()
    {
        var expense = new Expense("Food expense", Category.Food, 101.55m, Currency.Euro);
        var expenseItem = new ExpenseItem("Coffee", Category.Food, new Money(){Amount = 10.55m, Currency = Currency.Euro});
        
        expense.AddItem(expenseItem);
        
        Assert.Single(expense.ExpenseItems);
        Assert.Contains(expenseItem, expense.ExpenseItems);
        Assert.Equal(expenseItem.Expense, expense);
        return Task.CompletedTask;
    }
}