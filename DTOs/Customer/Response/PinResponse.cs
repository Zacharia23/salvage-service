using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Customer.Response;

public class PinResponse
{
    [JsonPropertyName("data")] public PinRequestData Data { get; set; }
}

public class PinRequestData
{
    [JsonPropertyName("pinId")] public string PinId { get; set; }

    [JsonPropertyName("message")] public MessageInfo Message { get; set; }
}

public class MessageInfo
{
    [JsonPropertyName("code")] public int Code { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }
}