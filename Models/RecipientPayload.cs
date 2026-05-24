using System.Text.Json.Serialization;

namespace SalvageCore.Models;

public class RecipientPayload
{
    [JsonPropertyName("recipient_id")] public int Id { get; set; }

    [JsonPropertyName("dest_addr")] public string Address { get; set; }
}