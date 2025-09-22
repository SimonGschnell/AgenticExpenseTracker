using System.ComponentModel;
using ExpenseTracker.Application.DTO.ExpenseDTO;
using ExpenseTracker.Application.Interfaces.Services;
using ModelContextProtocol.Server;
using Tesseract;

namespace ExpenseTracker.Tools;

[McpServerToolType]
public class Tool
{
    private readonly IExpenseService _service;
    public Tool(IExpenseService service)
    {
        _service = service;
    }

    [McpServerTool, Description("Creates an expense")]
    public async Task<string> CreateExpense(CreateExpenseDTO dto)
    {
        await _service.AddExpenseAsync(dto);
        return "tool was executed";
    }
    
    [McpServerTool, Description("Reads a receipt")]
    public string ReadReceipt(string receiptPath)
    {
        var tessDataPath = System.IO.Path.Combine(AppContext.BaseDirectory, "tessdata");
        using var engine = new TesseractEngine(tessDataPath, "deu_latf", EngineMode.Default);
        using var img = Pix.LoadFromFile(receiptPath);
        using var page = engine.Process(img, PageSegMode.SparseText);

        var text = page.GetText();
        var conf = page.GetMeanConfidence();

        Console.WriteLine($"Confidence: {conf:P0}");
        Console.WriteLine(text);
        return text;
    }
    
    [McpServerTool, Description("Gets the receipts from the system")]
    public List<string> GetReceipts()
    {
        var folderPath = @"D:\\Desktop\\RECEIPTS\\new";
        DirectoryInfo d = new DirectoryInfo(folderPath);
        if (!d.Exists) return [];
        
        var files = d.EnumerateFiles().Where(f=> f.Extension.ToLower() is ".png" or ".jpg" or ".jpeg").Select(f=> f.FullName).ToList();
        return files;
    }
}