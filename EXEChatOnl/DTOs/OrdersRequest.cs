using EXEChatOnl.Models;

namespace EXEChatOnl.DTOs;

public class OrdersRequest
{
    public int Id { get; set; }

    public string customerName { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}