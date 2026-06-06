namespace SalvageCore.DTOs.Offer.Response;

public class OfferStatisticsResponse
{
    public int TotalOffersCompleted { get; set; }
    public int VehiclesAvailable { get; set; }
    public int CustomersRegistered { get; set; }
    public int TotalSuccessfulBids { get; set; }
}
