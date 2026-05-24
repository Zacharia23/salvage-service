namespace SalvageCore.DTOs.Offer.Response;

public class BidResponse
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string BidReference { get; set; }
    public string VehicleNumber { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public Guid BidId { get; set; }
    public decimal SubmittedAmount { get; set; }
}