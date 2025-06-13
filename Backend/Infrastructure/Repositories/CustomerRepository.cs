using System.Data;
using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories;

public class CustomerRepository(IConfiguration configuration) : ICustomerRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration), "Database connection string 'DefaultConnection' not found.");

    private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

    public Customer? GetCustomerByUsername(string username)
    {
        const string sql = "SELECT * FROM customer WHERE Username = @Username;";
        using var connection = CreateConnection();
        return connection.QuerySingleOrDefault<Customer?>(sql, new { Username = username });
    }

    public void AddCustomer(Customer customer)
    {
        const string sql = "INSERT INTO customer (Username, PasswordHash) VALUES (@Username, @PasswordHash);";
        using var connection = CreateConnection();
        connection.Execute(sql, customer);
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        const string sql = "SELECT * FROM customer;";
        using var connection = CreateConnection();
        return connection.Query<Customer>(sql);
    }

    public void UpdateCustomer(Customer customer)
    {
        const string sql = @"UPDATE customer SET 
                                FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                PhoneNumber = @PhoneNumber 
                             WHERE CustomerId = @CustomerId;";
        using var connection = CreateConnection();
        connection.Execute(sql, customer);
    }
}