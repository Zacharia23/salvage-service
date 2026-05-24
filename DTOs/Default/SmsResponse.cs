using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Default;

public class SmsResponse
{
    [JsonPropertyName("successful")] public bool Successful { get; set; }

    [JsonPropertyName("request_id")] public string RequestId { get; set; }

    [JsonPropertyName("code")] public int Code { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }

    [JsonPropertyName("valid")] public int Valid { get; set; }

    [JsonPropertyName("invalid")] public int Invalid { get; set; }
}