namespace AgentLibrary.LLMRuntime.Ollama;

public class OllamaModels : ILLMModel
{
    private const string GptOssCloud120B = "gpt-oss:120b-cloud";
    private const string Llava = "llava";
    private const string Gemma12B = "gemma3:12b";
    private const string Qwen3LvCloud235B = "qwen3-vl:235b-cloud";
    public string ModelOptimizedForText => GptOssCloud120B;
    public string ModelOptimizedForImage => Qwen3LvCloud235B;
}