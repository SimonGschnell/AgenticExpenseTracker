using System.Text.Json;
using System.Text.Json.Serialization;
using AgentLibrary;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

[ApiController]
[Route("api/ExpenseTracker/[controller]")]
public class AgentController: ControllerBase
{
    private readonly Agent _agent;

    public AgentController(Agent agent)
    {
        _agent = agent;
    }
    [HttpPost("CreateExpenseFromImage")]
    public async Task<string> CreateExpenseFromImage([FromBody] string base64Image)
    {
        const string createExpensePrompt = "try to read the total sum spent in the receipt and create an expense in the expenseTracker application for me";
        return await _agent.SendPromptWithImageAsync( createExpensePrompt, base64Image);
        // var messages = _agent.GetChatLog();
        // var jsonSerializationOptions = new JsonSerializerOptions() { WriteIndented = true };
        // jsonSerializationOptions.Converters.Add(new JsonStringEnumConverter());
        // var serializedMessages = JsonSerializer.Serialize(messages,messages.GetType(), jsonSerializationOptions);
        // return serializedMessages;
    }
    
    [HttpPost("CreateExpenseWithItemsFromImage")]
    public async Task<string> CreateExpenseWithItemsFromImage([FromBody] string base64Image)
    {
        const string createExpensePrompt = """
                                           Read the receipt in the image.
                                           Verify that the sum of the expenseItems is equal to the total expense.
                                           Create an expense in the expenseTracker application with the total sum of the receipt.
                                           Create an expenseItem connected to the expense for every item on the receipt.
                                           """;
        return await _agent.SendPromptWithImageAsync( createExpensePrompt, base64Image);
    }
}