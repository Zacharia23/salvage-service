using Microsoft.EntityFrameworkCore;
using SalvageCore.Enums;

namespace SalvageCore.DTOs.Offer.Request;

public class OfferRequest
{
    public Guid VehicleId { get; set; }

    [Precision(8, 12)] public double IncrementPrice { get; set; } = 0;

    [Precision(8, 12)] public double InitialPrice { get; set; } = 0;

    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public Eligibility Eligibility { get; set; }
}