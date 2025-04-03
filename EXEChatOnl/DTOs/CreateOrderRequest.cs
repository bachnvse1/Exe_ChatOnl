namespace EXEChatOnl.DTOs;

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    
    public decimal TotalPrice { get; set; }

    public string? Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public List<CreateOrderDetailsRequest> OrderDetails { get; set; } = new();
    
    
}