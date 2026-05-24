using System.Text.Json.Serialization;

namespace SalvageCore.Models;

public class MessagePayload
{
    [JsonPropertyName("source_addr")] public string SourceAddress { get; set; }

    [JsonPropertyName("schedule_time")] public string ScheduledTime { get; set; }

    [JsonPropertyName("encoding")] public int Encoding { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }

    [JsonPropertyName("recipients")] public ICollection<RecipientPayload> Recipients { get; set; } = new List<RecipientPayload>();
}