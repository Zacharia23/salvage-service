using System.ComponentModel.DataAnnotations;

namespace SalvageCore.DTOs.Customer.Request;

public class CustomerRequest
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [EmailAddress] public string Email { get; set; } = string.Empty;

    [Phone] public string Phone { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}