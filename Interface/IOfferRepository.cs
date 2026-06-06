using SalvageCore.DTOs.Customer.Request;
using SalvageCore.DTOs.Offer.Request;
using SalvageCore.DTOs.Offer.Response;
using SalvageCore.Helpers;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface IOfferRepository
{
    public Task<Offer> CreateOffer(OfferRequest request);
    public Task<ICollection<OfferResponseList>> FetchOffers();
    public Task<OfferProfileResponse> FetchOfferDetails(Guid offerId);
    public Task<int> ExtendOffer(Guid offerId, string extendedDate);
    public Task<int> CompleteOffer(Guid offerId);
    public Task<Bid> SubmitBid(BidRequest request);
    public Task<Bid?> FetchLastBid(Guid offerId);
    public Task<ICollection<BidsResponseList>> FetchAllBids();
    public Task<ICollection<Bid>> FetchOfferBids(Guid offerId);
    public Task<int> AwardOffer(Guid offerId);
    public Task<ICollection<CustomerOfferResponse>> FetchActiveOffers();
    public Task<ServiceResponse<List<CustomerOfferResponse>>> FetchEndSoonOffers();
    public Task<ServiceResponse<List<CustomerOfferResponse>>> FetchCompletedOffers();
    public Task<ActiveOfferResponse?> FetchOfferProfile(Guid offerId);
    public Task<bool> OfferExists(Guid offerId);
    public Task<bool> BidExists(Guid bidId);
    public Task<ServiceResponse<bool>> Subscribe(SubscribeRequest request);
    public Task<ServiceResponse<bool>> Unsubscribe(SubscribeRequest request);
    public Task<ServiceResponse<bool>> CheckSubscriptionStatus(CheckSubscription request);
    public Task<ServiceResponse<OfferStatisticsResponse>> FetchStatistics();
}
