namespace SalvageCore.DTOs.Offer.Request;

public class BidRequest
{
    public Guid OfferId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal PreviousAmount { get; set; }
    public decimal SubmittedAmount { get; set; }
}