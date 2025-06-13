using Core.Models;

namespace Core.UseCases.Abstractions;


public interface IBurgerUseCases
{
    IEnumerable<Burger> GetAllBurgers();
    void AddBurger(Burger burger);
    void DeleteBurgerByName(string name);
}