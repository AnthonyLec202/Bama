using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface ICustomerRepository
{
    Customer? GetCustomerByUsername(string username);
    void AddCustomer(Customer customer);
    IEnumerable<Customer> GetAllCustomers();
    void UpdateCustomer(Customer customer);
}
