using Core.Models;
using Core.UseCases.Abstractions;
using Core.IGateways;

namespace Core.UseCases;

public class CustomerUseCases : ICustomerUseCases
{
    private readonly ICustomerGateway _customerGateway;

    public CustomerUseCases(ICustomerGateway customerGateway)
    {
        _customerGateway = customerGateway ?? throw new ArgumentNullException(nameof(customerGateway));
    }

    public Customer AuthenticateAndGetCustomer(AuthenticationRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Username and password are required.", nameof(request));
        }

        var customer = _customerGateway.GetCustomerByUsername(request.Username);
        if (customer == null)
        {
            throw new ArgumentException("Invalid username or password.");
        }

        var hashedPassword = _customerGateway.GetCustomerPasswordHash(request.Username);
        if (string.IsNullOrEmpty(hashedPassword))
        {
            throw new InvalidOperationException("Could not retrieve password for customer.");
        }

        if (BCrypt.Net.BCrypt.Verify(request.Password, hashedPassword))
        {
            return customer;
        }

        throw new ArgumentException("Invalid username or password.");
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        var customers = _customerGateway.GetAllCustomers();
        return customers;
    }

    public void Register(RegisterRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Invalid registration request");
        }

        if (request.Password != request.ConfirmPassword)
        {
            throw new ArgumentException("Passwords do not match");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        _customerGateway.AddCustomer(request.Username, hashedPassword);
    }
    
    public Customer? GetCustomerByUsername(string username)
{
    if (string.IsNullOrWhiteSpace(username))
        throw new ArgumentException("Le nom d'utilisateur est requis");

    return _customerGateway.GetCustomerByUsername(username);
}

public void UpdateCustomer(Customer customer)
{
    if (customer == null)
        throw new ArgumentNullException(nameof(customer));

    _customerGateway.UpdateCustomer(customer);
}

}
