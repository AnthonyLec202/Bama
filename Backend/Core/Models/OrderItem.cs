namespace Core.Models;

public class OrderItem
{
    public Burger Burger { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public decimal Price {get; set; }
}