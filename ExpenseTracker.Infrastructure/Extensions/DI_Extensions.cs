using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure.Extensions;

public static class DI_Extensions
{
    public static void AddInfrastructureServices(this IServiceCollection collection)
    {
        collection.AddScoped<IExpenseRepository, ExpenseRepository>();
    }
}