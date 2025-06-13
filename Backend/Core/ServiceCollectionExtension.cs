using Core.UseCases.Abstractions;
using Core.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddTransient<ICustomerUseCases, CustomerUseCases>();
        services.AddTransient<IBurgerUseCases, BurgerUseCases>();
        services.AddTransient<ICustomerOrderUseCases, CustomerOrderUseCases>();
    
        return services;
    }
}