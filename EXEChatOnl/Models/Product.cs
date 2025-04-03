using System;
using System.Collections.Generic;

namespace EXEChatOnl.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? ImageUrl { get; set; }

    public string? ShopeeUrl { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }
    
    public bool IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
