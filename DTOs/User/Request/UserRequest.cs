using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.User.Request;

public class UserRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;
}
