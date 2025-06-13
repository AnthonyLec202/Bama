using Core.Models;
using Core.IGateways;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class BurgerUseCases : IBurgerUseCases
{
    private readonly IBurgerGateway _burgergateway;

    public BurgerUseCases(IBurgerGateway burgergateway)
    {
        _burgergateway = burgergateway;
    }

    public IEnumerable<Burger> GetAllBurgers()
    {
        return _burgergateway.GetAllBurgers();
    }

    public void AddBurger(Burger burger)
    {
        if (burger.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.", nameof(burger.Price));
        }

        if (burger.Stock < 0)
        {
            throw new ArgumentException("Product stock cannot be negative.", nameof(burger.Stock));
        }

        _burgergateway.AddBurger(burger);
    }

    public void DeleteBurgerByName(string name)
    {
        _burgergateway.DeleteBurgerByName(name);
    }
}