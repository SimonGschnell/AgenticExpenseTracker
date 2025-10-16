using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Services;

namespace ExpenseTracker.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;
public static class DI_Extension
{
    public static void AddApplicationServices(this IServiceCollection collection)
    {
        collection.AddScoped<IExpenseService, ExpenseService>();
    }
}