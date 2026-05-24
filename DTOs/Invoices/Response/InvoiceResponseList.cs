namespace SalvageCore.DTOs.Invoices.Response;

public class InvoiceResponseList
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public Guid BidId { get; set; }
    public string BidReference { get; set; }
    public string VehicleNumber { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public decimal Amount { get; set; }
}