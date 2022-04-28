using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialChat.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}