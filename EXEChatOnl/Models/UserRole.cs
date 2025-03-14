using System;
using System.Collections.Generic;

namespace EXEChatOnl.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
