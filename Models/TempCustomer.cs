namespace SalvageCore.Models;

public class TempCustomer
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Username { get; set; } = string.Empty;
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required string Phone { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public string? OtpCode { get; set; }
    public DateTime OtpExpiry { get; set; }
    public int OtpAttempts { get; set; }
    public bool IsSmsSent { get; set; }
    public DateTime? CreatedDate { get; set; }
}