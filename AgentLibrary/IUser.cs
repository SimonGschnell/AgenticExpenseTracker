namespace AgentLibrary;

public interface IUser
{
    public Task<string> Prompt(string input);
    Task<string> PromptWithImage(string input, string imagePath);
}