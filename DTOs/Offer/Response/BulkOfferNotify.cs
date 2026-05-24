using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Offer.Response;

public class BulkOfferNotify
{
    [JsonPropertyName("messages")] public ICollection<BidMessage> Messages { get; set; } = new List<BidMessage>();

    [JsonPropertyName("flash")] public int Flash { get; set; } = 0;

    [JsonPropertyName("reference")] public string Reference { get; set; } = string.Empty;
}