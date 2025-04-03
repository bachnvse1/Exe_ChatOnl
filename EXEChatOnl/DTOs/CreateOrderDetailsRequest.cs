namespace EXEChatOnl.DTOs;

public class CreateOrderDetailsRequest
{
    public int orderId { get; set; }
    public int ProductId { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
}