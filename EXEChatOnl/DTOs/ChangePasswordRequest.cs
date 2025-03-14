using System.ComponentModel.DataAnnotations;

namespace EXEChatOnl.DTOs;

public class ChangePasswordRequest
{
    [Required]
    public string Password { get; set; }

    [Required]
    public string NewPassword { get; set; }

    [Required]
    [Compare("NewPassword", ErrorMessage = "Confirm Password must match New Password")]
    public string ConfirmPassword { get; set; }
}
