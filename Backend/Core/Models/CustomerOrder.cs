namespace Core.Models;

public class CustomerOrder
{
    public int CustomerOrderId { get; set; }
    public decimal TotalPrice => OrderItems.Sum(item => item.Quantity * item.Price);
    public IEnumerable<OrderItem> OrderItems { get; set; } = [];
    public string Status { get; set; } = "InProgress";
}