using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Customer.Request;

public class CodeRequest
{
    [JsonPropertyName("appId")] public int AppId { get; set; }

    [JsonPropertyName("msisdn")] public string Msisdn { get; set; } = string.Empty;
}