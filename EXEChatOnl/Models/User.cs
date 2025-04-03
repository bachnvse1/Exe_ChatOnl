using System;
using System.Collections.Generic;

namespace EXEChatOnl.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; } = null!;

    public string? Password { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
    public bool IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
}
