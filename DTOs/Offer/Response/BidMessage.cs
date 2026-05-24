using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Offer.Response;

public class BidMessage
{
    [JsonPropertyName("from")] public string From { get; set; } = string.Empty;

    [JsonPropertyName("to")] public string To { get; set; } = string.Empty;

    [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;
}