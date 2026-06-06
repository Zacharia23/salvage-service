using System.Text.Json.Serialization;

namespace SalvageCore.DTOs.Customer.Request;

public class CheckSubscription
{
    [JsonIgnore]
    public Guid CustomerId { get; set; }
    public Guid OfferId { get; set; }
}
