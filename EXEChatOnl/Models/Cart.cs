﻿using System;
using System.Collections.Generic;

namespace EXEChatOnl.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
