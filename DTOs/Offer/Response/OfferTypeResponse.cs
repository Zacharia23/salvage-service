namespace SalvageCore.DTOs.Offer.Response;

public class OfferTypeResponse
{
    public Guid OfferTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}