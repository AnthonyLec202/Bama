namespace Infrastructure.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int CustomerOrderId { get; set; }
    public int BurgerId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    public Burger? Burger { get; set; }
}