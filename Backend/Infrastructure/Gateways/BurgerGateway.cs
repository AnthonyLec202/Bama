using Core.IGateways;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Gateways;

public class BurgerGateway : IBurgerGateway
{
    private readonly IBurgerRepository _burgerRepository;

    public BurgerGateway(IBurgerRepository burgerRepository)
    {
        _burgerRepository = burgerRepository;
    }

   public IEnumerable<Core.Models.Burger> GetAllBurgers()
{
    var infraBurgers = _burgerRepository.GetAllBurgers();
     return infraBurgers.Select(b => new Core.Models.Burger
    {
        BurgerId = b.BurgerId,
        Name = b.Name,
        Description = b.Description,
        Price = b.Price,
        Stock = (int)(b.Stock - _burgerRepository.GetQuantityOfBurgerInOrders(b.BurgerId)),
        ImageUrl = b.ImageUrl
    });
}

    public void AddBurger(Core.Models.Burger burger)
    {
        var infraBurger = new Burger
        {
            Name = burger.Name,
            Description = burger.Description,
            Price = burger.Price,
            Stock = burger.Stock,
        };
        _burgerRepository.AddBurger(infraBurger);
    }

    public void DeleteBurgerByName(string name)
    {

        _burgerRepository.DeleteBurgerByName(name);
    }

    public Core.Models.Burger? GetBurgerById(int burgerId)
{
    var infraBurger = _burgerRepository.GetBurgerById(burgerId);
    if (infraBurger == null)
    {
        return null;
    }

    return new Core.Models.Burger
    {
        BurgerId = infraBurger.BurgerId,
        Name = infraBurger.Name,
        Description = infraBurger.Description,
        Price = infraBurger.Price,
        Stock = (int)(infraBurger.Stock - _burgerRepository.GetQuantityOfBurgerInOrders(infraBurger.BurgerId)),
        ImageUrl = infraBurger.ImageUrl
    };
}


}