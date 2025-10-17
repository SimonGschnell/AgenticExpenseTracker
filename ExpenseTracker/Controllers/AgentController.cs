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
    [HttpPost]
    public async Task<string> CreateExpenseFromImage([FromBody] string base64Image)
    {
        const string createExpensePrompt = "try to read the total sum spent in the receipt and create a expense in the expenseTracker application for me";
        return await _agent.SendPromptWithImageAsync( createExpensePrompt, base64Image);
        // var messages = _agent.GetChatLog();
        // var jsonSerializationOptions = new JsonSerializerOptions() { WriteIndented = true };
        // jsonSerializationOptions.Converters.Add(new JsonStringEnumConverter());
        // var serializedMessages = JsonSerializer.Serialize(messages,messages.GetType(), jsonSerializationOptions);
        // return serializedMessages;
    }
}

public record PromptWithImage(string Instruction, string Base64Image);
