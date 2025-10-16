namespace AgentLibrary;

public class User(Agent agent) : IUser
{

    public async Task<string> Prompt(string input)
    {
        return await agent.SendPromptAsync(input);
    }

    public async Task<string> PromptWithImage(string input, string imagePath)
    {
        return await agent.SendPromptWithImageAsync(input, imagePath);
    }
}