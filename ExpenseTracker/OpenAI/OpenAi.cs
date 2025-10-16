using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace ExpenseTracker.OpenAI;

public class OpenAi
{
    private readonly OpenAIClient _client;
    private readonly List<ChatMessage> _messages = [];

    private OpenAi(OpenAIClient client, ChatMessage[] messages)
    {
        _client = client;
        foreach (var message in messages)
        {
            _messages.Add(message);
        }
    }

    public static OpenAi Create(string apiKey)
    {
        var client = new OpenAIClient(apiKey);
        
        var messages = new ChatMessage[]
        {
            new SystemChatMessage("You are a helpful assistant")
        };
        
        return new OpenAi(client, messages);
    }
    
    public void AddUserMessage(string message)
    {
        _messages.Add(new UserChatMessage(message));
    }
    
    public void AddSystemMessage(string message)
    {
        _messages.Add(new SystemChatMessage(message));
    }
    
    public void AddAssistantMessage(string message)
    {
        _messages.Add(new AssistantChatMessage(message));
    }
    
    public async Task<string> GetResponse()
    {
        var response = await _client.GetChatClient("gpt-5").CompleteChatAsync(_messages);
        return string.Join("", response.Value.Content.Select(c => c.Text));
    }
}