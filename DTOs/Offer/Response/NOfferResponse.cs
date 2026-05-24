using Microsoft.EntityFrameworkCore;
using SalvageCore.DTOs.Status.Response;
using SalvageCore.DTOs.Vehicle.Response;

namespace SalvageCore.DTOs.Offer.Response;

public class NOfferResponse
{
    public Guid OfferId { get; set; } = Guid.NewGuid();
    public string ReferenceNumber { get; set; } = string.Empty;
    public virtual VehicleResponse? Vehicle { get; set; }
    public virtual OfferTypeResponse? OfferType { get; set; }

    [Precision(8, 12)] public double IncrementPrice { get; set; }

    [Precision(8, 12)] public double InitialPrice { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public virtual StatusResponse? Status { get; set; }
    public DateTime CreatedDate { get; set; }
}