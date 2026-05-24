using System.Text.Json.Serialization;

namespace SalvageCore.Models;

public class NotificationPayload
{
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;

    [JsonPropertyName("recipients")] public ICollection<string> Recipients { get; set; } = null!;

    [JsonPropertyName("sender_id")] public string SenderId { get; set; } = string.Empty;

    [JsonPropertyName("campaign_id")] public string CampaignId { get; set; } = string.Empty;
}