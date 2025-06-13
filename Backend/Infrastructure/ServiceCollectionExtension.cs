
using Core.IGateways;
using Core.Models;
using Infrastructure.Gateways;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IBurgerRepository, BurgerRepository>();
        services.AddTransient<ICustomerOrderRepository, CustomerOrderRepository>();
        services.AddTransient<ICustomerGateway, CustomerGateway>();
        services.AddTransient<IBurgerGateway, BurgerGateway>();
        services.AddTransient<ICustomerOrderGateway, CustomerOrderGateway>();
    
        return services;
    }
}
