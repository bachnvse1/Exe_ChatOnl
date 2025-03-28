namespace EXEChatOnl.DTOs;

public class ProfileUserRequest
{
    public string Username { get; set; } = null!;
    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public List<string>? RoleName { get; set; } = null!;

}