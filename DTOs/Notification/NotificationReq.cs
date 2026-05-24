using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Notification;

public class NotificationReq
{
    [JsonPropertyName("from")] public required string? From { get; set; }

    [JsonPropertyName("to")] public required string To { get; set; }

    [JsonPropertyName("text")] public required string Text { get; set; }

    [JsonPropertyName("flash")] public required int Flash { get; set; }

    [JsonPropertyName("reference")] public required string Reference { get; set; }
}