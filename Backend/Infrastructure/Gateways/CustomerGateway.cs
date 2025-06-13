using Core.IGateways;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class CustomerGateway : ICustomerGateway
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerGateway(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public void AddCustomer(string username, string passwordHash)
    {
        var customer = new Infrastructure.Models.Customer { Username = username, PasswordHash = passwordHash };
        _customerRepository.AddCustomer(customer);
    }

    public string? GetCustomerPasswordHash(string username)
    {
        var customer = _customerRepository.GetCustomerByUsername(username);
        return customer?.PasswordHash;
    }

    public IEnumerable<Core.Models.Customer> GetAllCustomers()
    {
        var customers = _customerRepository.GetAllCustomers();
        return customers.Select(customer => new Core.Models.Customer
        {
            CustomerId = customer.CustomerId,
            Username = customer.Username,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Role = customer.Role,
            PasswordHash = customer.PasswordHash
        });
    }

    public Core.Models.Customer? GetCustomerByUsername(string username)
    {
        var infraCustomer = _customerRepository.GetCustomerByUsername(username);
        if (infraCustomer == null) return null;

        return new Core.Models.Customer
        {
            CustomerId = infraCustomer.CustomerId,
            Username = infraCustomer.Username,
            FirstName = infraCustomer.FirstName,
            LastName = infraCustomer.LastName,
            Email = infraCustomer.Email,
            PhoneNumber = infraCustomer.PhoneNumber,
            Role = infraCustomer.Role,
            PasswordHash = infraCustomer.PasswordHash
        };
    }

    public void UpdateCustomer(Core.Models.Customer customer)
    {
        var existingCustomer = _customerRepository.GetCustomerByUsername(customer.Username);
        if (existingCustomer == null)
        {
            return;
        }

        existingCustomer.FirstName = customer.FirstName;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.Email = customer.Email;
        existingCustomer.PhoneNumber = customer.PhoneNumber;

        _customerRepository.UpdateCustomer(existingCustomer);
    }
}