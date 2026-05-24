namespace SalvageCore.DTOs.Customer.Request;

public class CheckSubscription
{
    public Guid CustomerId { get; set; }
    public Guid OfferId { get; set; }
}