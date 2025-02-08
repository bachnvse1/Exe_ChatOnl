using System;
using System.Collections.Generic;

namespace EXEChatOnl.Models;

public partial class Chat
{
    public int Id { get; set; }

    public string SessionId { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string Sender { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
