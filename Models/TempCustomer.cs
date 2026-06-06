namespace SalvageCore.Models;

public class TempCustomer
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string ApplicationUserId { get; set; } = string.Empty;
    public required string Phone { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required string OtpHash { get; set; } = string.Empty;
    public Enums.AuthChallengePurpose Purpose { get; set; }
    public DateTime OtpExpiry { get; set; }
    public int OtpAttempts { get; set; }
    public DateTime LastSentAt { get; set; }
    public DateTime? ConsumedAt { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
