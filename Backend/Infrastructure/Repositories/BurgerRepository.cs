using System.Data;
using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories;

public class BurgerRepository(IConfiguration configuration) : IBurgerRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");

    private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

    public IEnumerable<Burger> GetAllBurgers()
    {
        const string sql = "SELECT BurgerId, Name, Description, Price, Stock, ImageUrl FROM Burger;";
        using var connection = CreateConnection();
        return connection.Query<Burger>(sql);
    }

    public Burger? GetBurgerById(int burgerId)
    {
        const string sql = "SELECT BurgerId, Name, Description, Price, Stock, ImageUrl FROM Burger WHERE BurgerId = @BurgerId;";
        using var connection = CreateConnection();
        return connection.QuerySingleOrDefault<Burger>(sql, new { BurgerId = burgerId });
    }

    public void AddBurger(Burger burger)
    {
        const string sql = "INSERT INTO Burger (Name, Description, Price, Stock, ImageUrl) VALUES (@Name, @Description, @Price, @Stock, @ImageUrl);";
        using var connection = CreateConnection();
        connection.Execute(sql, burger);
    }

    public void DeleteBurgerByName(string name)
    {
        const string sql = "DELETE FROM Burger WHERE Name = @Name";
        using var connection = CreateConnection();
        connection.Execute(sql, new { Name = name });
    }

    public int GetQuantityOfBurgerInOrders(int burgerId)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT SUM(Quantity) FROM OrderItem WHERE BurgerId = @BurgerId";
        var quantity = connection.QueryFirstOrDefault<int?>(sql, new { BurgerId = burgerId });
        return quantity ?? 0;
    }

    public void UpdateStock(int burgerId, int quantityToDecrement)
    {
        const string sql = "UPDATE Burger SET Stock = Stock - @QuantityToDecrement WHERE BurgerId = @BurgerId;";
        using var connection = CreateConnection();
        connection.Execute(sql, new { QuantityToDecrement = quantityToDecrement, BurgerId = burgerId });
    }
    
}