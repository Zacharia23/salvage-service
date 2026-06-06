namespace SalvageCore.DTOs.Customer.Response;

public class RegistrationResponse
{
    public Guid VerificationId { get; set; }
    public DateTime ExpiresAt { get; set; }
}
