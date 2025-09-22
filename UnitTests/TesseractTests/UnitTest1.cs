using Tesseract;

namespace UnitTests;

public class UnitTest1
{
    [Theory]
    [InlineData("testReceipt.jpeg")]
    public void Test1(string filePath)
    {
        var tessDataPath = Path.Combine(AppContext.BaseDirectory, $"TesseractTests\\tessdata");
        using var engine = new TesseractEngine(tessDataPath, "deu", EngineMode.Default);
        using var img = Pix.LoadFromFile(Path.Combine(AppContext.BaseDirectory, $"TesseractTests\\{filePath}"));
        using var page = engine.Process(img, PageSegMode.SparseText);

        var text = page.GetText();

        Assert.Contains("5.82", text);
    }
}