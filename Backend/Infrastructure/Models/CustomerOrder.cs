namespace Infrastructure.Models;


public class CustomerOrder
{
    public int CustomerOrderId { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    public string Status { get; set; } = "InProgress";
}