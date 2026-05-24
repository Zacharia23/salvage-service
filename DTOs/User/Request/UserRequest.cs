using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.User.Request;

public class UserRequest
{
    public string Username { get; set; } = string.Empty;

    [EmailAddress] public string Email { get; set; } = string.Empty;

    [Phone] public string Phone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}