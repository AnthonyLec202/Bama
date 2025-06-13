using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions
{
    public interface ICustomerOrderRepository
    {
        CustomerOrder? GetCustomerOrderByCustomerId(int customerId);
        CustomerOrder CreateCustomerOrder(int customerId);
        OrderItem? GetOrderItemById(int customerId, int burgerId);
        void AddItemsToCustomerOrder(int customerOrderId, IEnumerable<OrderItem> items);
        void UpdateItemsInCustomerOrder(int customerOrderId, IEnumerable<OrderItem> items);
        void ClearCustomerOrder(int customerOrderId);

        void UpdateOrderStatus(int customerOrderId, string status);

        IEnumerable<CustomerOrder> GetAllOrdersByCustomerId(int customerId);
    }
}
