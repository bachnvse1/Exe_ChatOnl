using EXEChatOnl.Models;

namespace EXEChatOnl.DTOs;

public class OrderDetailsRequest
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string productName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Product Product { get; set; } = null!;
}