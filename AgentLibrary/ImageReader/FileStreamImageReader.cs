namespace AgentLibrary.ImageReader;

public class FileStreamImageReader : IImageReader
{
    public string GetBase64FromImageUrl(string imageUrl)
    {
        var imageBytes = File.ReadAllBytes(imageUrl);
        return Convert.ToBase64String(imageBytes);
    }
}