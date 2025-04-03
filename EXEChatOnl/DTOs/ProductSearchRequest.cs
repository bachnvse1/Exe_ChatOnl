using EXEChatOnl.Models;

namespace EXEChatOnl.DTOs;

public class ProductSearchRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }

    public string ShopeeUrl { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public int? CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category? Category { get; set; }
    public string NormalizedName { get; set; }
}