using SalvageCore.Enums;

namespace SalvageCore.DTOs.Offer.Request;

public class SubscribeRequest
{
    public Guid CustomerId { get; set; }
    public Guid OfferId { get; set; }
    public SubPreference Preference { get; set; }
}