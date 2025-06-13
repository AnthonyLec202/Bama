using Core.Models;

namespace Core.IGateways;

public interface ICustomerOrderGateway
{
    CustomerOrder? GetCustomerOrderByCustomerId(int customerId);
    IEnumerable<CustomerOrder> GetAllOrdersByCustomerId(int customerId);
    CustomerOrder CreateCustomerOrder(int customerId);
    void AddOrUpdateItemsToCustomerOrder(int customerOrderId, IEnumerable<OrderItem> items);
    void ClearCustomerOrder(int customerOrderId);
    void FinalizeOrder(CustomerOrder order);
}