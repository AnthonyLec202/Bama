using Core.Models;

namespace Core.UseCases.Abstractions;

public interface ICustomerUseCases
{
    Customer AuthenticateAndGetCustomer(AuthenticationRequest request);
    void Register(RegisterRequest request);
    IEnumerable<Customer> GetAllCustomers();

    Customer? GetCustomerByUsername(string username);
void UpdateCustomer(Customer customer);

}
