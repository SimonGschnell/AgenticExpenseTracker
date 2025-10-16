using System.Security.Authentication;
using System.Text.Json.Serialization;
using AgentLibrary;
using DotNetEnv;
using ExpenseTracker.Application.Extensions;
using ExpenseTracker.Infrastructure.Context;
using ExpenseTracker.Infrastructure.Extensions;
using ExpenseTracker.LLMRuntime;
using ExpenseTracker.LLMRuntime.Ollama;
using ExpenseTracker.OpenAI;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Env.Load();
        builder.Services.Configure<OpenAiSettings>(options =>
        {
            options.ApiKey = Environment.GetEnvironmentVariable("OpenAI__ApiKey") ?? 
                             throw new InvalidCredentialException("Invalid OpenApi Key");
        });
        // Add services to the container.

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddInfrastructureServices();
        builder.Services.AddApplicationServices();
        builder.Services.AddScoped<ILLMAbstractFactory, OllamaAbstractFactory>();
        builder.Services.AddAgentCommands();
        builder.Services.AddScoped<Agent>(sp =>
        {
            var agent = new Agent(sp.GetRequiredService<ILLMAbstractFactory>());
            foreach (var command in sp.GetServices<ICommand>())
            {
                agent.AddTool(command);
            }
            return agent;
        });
        
        //mcp server
        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}