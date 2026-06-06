using System.ComponentModel.DataAnnotations;
using SalvageCore.DTOs.Customer.Response;

namespace SalvageCore.DTOs.User;

public class LoginResponse
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public string? Username { get; set; }

    [EmailAddress] public string? Email { get; set; }

    [Phone] public string? Phone { get; set; }

    public string? Role { get; set; }
    public CustomerInfo? Details { get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public bool ProfileComplete { get; set; }
}
