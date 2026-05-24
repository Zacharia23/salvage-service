using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Customer.Request;

public class VerifyPayload
{
    [JsonPropertyName("pinId")] public string PinId { get; set; } = string.Empty;

    [JsonPropertyName("pin")] public string Code { get; set; } = string.Empty;
}