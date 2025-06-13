using Core.Models;

namespace Core.IGateways;

public interface IBurgerGateway
{
    IEnumerable<Burger> GetAllBurgers();
    void AddBurger(Burger burger);
    void DeleteBurgerByName(string name);

    Burger? GetBurgerById(int burgerId);
}
