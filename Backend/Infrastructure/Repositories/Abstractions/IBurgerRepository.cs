using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IBurgerRepository
{
    IEnumerable<Burger> GetAllBurgers();
    Burger? GetBurgerById(int burgerId);
    void AddBurger(Burger burger);
    void DeleteBurgerByName(string name);
    int GetQuantityOfBurgerInOrders(int burgerId);
    void UpdateStock(int burgerId, int quantityToDecrement);

} 