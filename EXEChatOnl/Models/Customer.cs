﻿using System;
using System.Collections.Generic;

namespace EXEChatOnl.Models;

public partial class Customer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}
