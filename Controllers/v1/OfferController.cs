using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalvageCore.DTOs.Offer.Request;
using SalvageCore.Extensions;
using SalvageCore.Interface;
using Serilog;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class OfferController : ControllerBase
{
    private readonly IOfferRepository _offer;

    public OfferController(IOfferRepository offer)
    {
        _offer = offer;
    }

    [HttpPost]
    [Route("[Controller]/CreateOffer")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<IActionResult> CreateOffers([FromBody] OfferRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var offer = await _offer.CreateOffer(request);

        return this.Respond("Offer Created Successfully", offer.ReferenceNumber);
    }

    [HttpGet]
    [Route("[Controller]/FetchOffers")]
    public async Task<IActionResult> FetchOffers()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var offers = await _offer.FetchOffers();

        return this.Respond("Offers Fetched Successfully", offers);
    }

    [HttpGet]
    [Route("[Controller]/FetchStatistics")]
    [AllowAnonymous]
    public async Task<IActionResult> FetchStatisticsAction()
    {
        var result = await _offer.FetchStatistics();

        if (!result.Success)
            return this.RespondError(
                StatusCodes.Status500InternalServerError,
                result.Message ?? result.Errors.FirstOrDefault() ?? "Failed to fetch offer statistics.");

        return this.Respond(result.Message ?? "Offer statistics fetched successfully", result.Data!);
    }

    [HttpGet]
    [Route("[Controller]/FetchOfferProfile/{offerId:guid}")]
    public async Task<IActionResult> FetchOfferDetails([FromRoute] Guid offerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _offer.OfferExists(offerId)) return this.RespondNotFound($"Offer {offerId} Not Found");

        var result = await _offer.FetchOfferDetails(offerId);

        return this.Respond("Offer Details Fetched Successfully", result);
    }

    [HttpPut]
    [Route("[Controller]/ExtendOffer")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<IActionResult> ExtendOfferAction([FromBody] ExtendRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _offer.OfferExists(request.OfferId)) return this.RespondNotFound($"Offer {request.OfferId} Not Found");

        await _offer.ExtendOffer(request.OfferId, request.ExtendedDate);

        return this.Respond("Offer End Date Extended", request.ExtendedDate);
    }

    [HttpPut]
    [Route("[Controller]/CompleteOffer/{offerId:guid}")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<IActionResult> CompleteOfferAction([FromRoute] Guid offerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _offer.OfferExists(offerId)) return this.RespondNotFound($"Offer {offerId} Not Found");

        await _offer.CompleteOffer(offerId);

        return this.Respond("Offer Completed Successfully", null);
    }

    [HttpPost]
    [Route("[Controller]/SubmitOfferBid")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> SubmitOfferBid([FromBody] BidRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!TryGetCustomerId(out var customerId))
            return this.RespondError(StatusCodes.Status401Unauthorized, "Customer identity is missing.");

        request.CustomerId = customerId;

        // Check Offer Details
        var offer = await _offer.FetchOfferDetails(request.OfferId);

        // Check Last Submitted Bid for that respective offer
        var lastBid = await _offer.FetchLastBid(request.OfferId);

        Log.Information("Last acquired bid => {@lastBid}", lastBid);

        var bidValidity = ValidateOffer(lastBid is null ? offer.InitialPrice : Convert.ToDouble(lastBid.SubmittedAmount), offer.IncrementPrice, request.SubmittedAmount);

        // Check Bid Validity
        if (!bidValidity) return this.RespondError(StatusCodes.Status400BadRequest, "Submitted Amount must be within incremental price or higher");

        var result = await _offer.SubmitBid(request);

        return this.Respond("Offer Submitted Successfully", result.SubmittedAmount);
    }

    [HttpGet]
    [Route("[Controller]/FetchAllBids")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<IActionResult> FetchBidsAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _offer.FetchAllBids();

        return this.Respond("Bids Fetched Successfully", result);
    }

    [HttpGet]
    [Route("[Controller]/FetchOfferBids/{offerId:guid}")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<IActionResult> FetchOfferBidsAction([FromRoute] Guid offerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _offer.OfferExists(offerId)) return this.RespondNotFound($"Offer {offerId} Not Found");

        var results = await _offer.FetchOfferBids(offerId);

        return this.Respond("Offers Fetched Successfully", results);
    }

    [HttpPut]
    [Route("[Controller]/AwardCurrentBid/{bidId:guid}")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<IActionResult> AwardBidAction([FromRoute] Guid bidId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _offer.BidExists(bidId)) return this.RespondNotFound($"Bid {bidId} Not Found");

        var results = await _offer.AwardOffer(bidId);

        return this.Respond("Bids Fetched Successfully", results);
    }

    [HttpPost]
    [Route("[Controller]/Subscribe")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> SubscribeOfferAction([FromBody] SubscribeRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!TryGetCustomerId(out var customerId))
            return this.RespondError(StatusCodes.Status401Unauthorized, "Customer identity is missing.");

        request.CustomerId = customerId;
        var response = await _offer.Subscribe(request);

        if (!response.Success)
            return this.RespondError(StatusCodes.Status400BadRequest, response.Message ?? "Unable to subscribe to offer.");

        return this.Respond(response.Message!, response.Data);
    }

    [HttpPost]
    [Route("[Controller]/Unsubscribe")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UnSubscribeOfferAction([FromBody] SubscribeRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!TryGetCustomerId(out var customerId))
            return this.RespondError(StatusCodes.Status401Unauthorized, "Customer identity is missing.");

        request.CustomerId = customerId;
        var response = await _offer.Unsubscribe(request);

        if (!response.Success)
            return this.RespondError(StatusCodes.Status400BadRequest, response.Message ?? "Unable to unsubscribe from offer.");

        return this.Respond(response.Message!, response.Data);
    }

    private bool ValidateOffer(double initialPrice, double incrementPrice, decimal submittedAmount)
    {
        return submittedAmount > (decimal)initialPrice && (submittedAmount - (decimal)initialPrice) % (decimal)incrementPrice == 0;
    }

    private bool TryGetCustomerId(out Guid customerId)
    {
        return Guid.TryParse(User.FindFirstValue("customer_id"), out customerId);
    }
}
