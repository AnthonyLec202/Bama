namespace Api.Models;

public class OrderItem
{
    public int BurgerId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; set; }
}
 