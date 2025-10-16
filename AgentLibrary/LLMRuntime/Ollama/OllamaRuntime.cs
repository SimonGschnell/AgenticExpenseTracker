using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExpenseTracker.LLMRuntime;
using ExpenseTracker.LLMRuntime.Ollama;

namespace AgentLibrary.LLMRuntime.Ollama;

public class OllamaRuntime: IDisposable, ILLMRuntime
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    public static OllamaRuntime Create(string baseAddress)
    {
        var client = new HttpClient { BaseAddress = new Uri(baseAddress) };
        return new OllamaRuntime(client);
    }

    private OllamaRuntime(HttpClient client)
    {
        _client = client;
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    

    public async Task<ILLMRunTimeResponse> SendPrompt(ILLMChatLog chat)
    {
        var requestJson = JsonSerializer.Serialize(chat.createLlmRequest(), _jsonSerializerOptions);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
        
        
        var stringResponse = await _client.PostAsync("api/chat", content);
        stringResponse.EnsureSuccessStatusCode();

        ILLMRunTimeResponse response = JsonSerializer.Deserialize<OllamaResponse>(await stringResponse.Content.ReadAsStringAsync()) ?? new OllamaResponse(){};
        return response;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

}