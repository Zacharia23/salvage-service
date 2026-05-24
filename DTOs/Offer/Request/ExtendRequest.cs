namespace SalvageCore.DTOs.Offer.Request;

public class ExtendRequest
{
    public Guid OfferId { get; set; }
    public string ExtendedDate { get; set; } = string.Empty;
}