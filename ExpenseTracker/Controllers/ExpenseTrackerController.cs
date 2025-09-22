using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseTrackerController: ControllerBase
{
    private IExpenseService _expenseService;
    public ExpenseTrackerController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody]CreateExpenseDTO dto)
    {
        await _expenseService.AddExpenseAsync(dto);
        return Ok("Expense added successfully");
    }
}