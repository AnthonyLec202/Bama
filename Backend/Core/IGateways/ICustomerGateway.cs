using Core.Models;

namespace Core.IGateways;

public interface ICustomerGateway
{
    string? GetCustomerPasswordHash(string username);
    Customer? GetCustomerByUsername(string username);
    void AddCustomer(string username, string passwordHash);
    IEnumerable<Customer> GetAllCustomers();
    void UpdateCustomer(Core.Models.Customer customer);     
}
