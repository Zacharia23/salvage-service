using System.Globalization;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Customer.Request;
using SalvageCore.DTOs.Offer.Request;
using SalvageCore.DTOs.Offer.Response;
using SalvageCore.DTOs.Vehicle.Response;
using SalvageCore.Enums;
using SalvageCore.Helpers;
using SalvageCore.Interface;
using SalvageCore.Models;
using SalvageCore.Service;
using Serilog;

namespace SalvageCore.Repository;

public class OfferRepository : IOfferRepository
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly IPdfService _pdfService;

    public OfferRepository(ApplicationDbContext context, IConfiguration configuration, IPdfService pdfService)
    {
        _context = context;
        _configuration = configuration;
        _pdfService = pdfService;
    }

    public async Task<Offer> CreateOffer(OfferRequest request)
    {
        try
        {
            var generator = new NumberGenerator(_context);
            var number = generator.GenerateReference();

            var results = new Offer
            {
                ReferenceNumber = number,
                EntityType = EntityTypeEnum.Vehicle,
                Eligibility = request.Eligibility,
                VehicleId = request.VehicleId,
                IncrementPrice = request.IncrementPrice,
                ReservePrice = request.InitialPrice,
                StartDate = DateTime.SpecifyKind(DateTime.Parse(request.StartDate, null, DateTimeStyles.RoundtripKind), DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(DateTime.Parse(request.EndDate, null, DateTimeStyles.RoundtripKind), DateTimeKind.Utc)
            };

            await _context.Offers.AddAsync(results);
            await _context.SaveChangesAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to create offer => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<OfferResponseList>> FetchOffers()
    {
        try
        {
            var results = await _context.Offers
                .Select(x => new OfferResponseList
                {
                    OfferId = x.Id,
                    ReferenceNumber = x.ReferenceNumber,
                    PlateNumber = x.Vehicle.RegistrationNumber,
                    VehicleNumber = x.Vehicle.Reference,
                    Make = x.Vehicle.Make.Name,
                    Model = x.Vehicle.Model.Name,
                    OfferType = x.OfferNature.ToString(),
                    InitialPrice = x.ReservePrice,
                    IncrementPrice = x.IncrementPrice,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TotalBids = x.Bids.Count,
                    CreatedDate = x.CreatedDate
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch offers => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<OfferProfileResponse> FetchOfferDetails(Guid offerId)
    {
        try
        {
            var results = await _context.Offers
                .Where(o => o.Id.Equals(offerId))
                .Select(x => new OfferProfileResponse
                {
                    OfferId = x.Id,
                    ReferenceNumber = x.ReferenceNumber,
                    PlateNumber = x.Vehicle.RegistrationNumber,
                    VehicleNumber = x.Vehicle.Reference,
                    Make = x.Vehicle.Make.Name,
                    Model = x.Vehicle.Model.Name,
                    InitialPrice = x.ReservePrice,
                    IncrementPrice = x.IncrementPrice,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedDate = x.CreatedDate,
                    Bids = x.Bids
                        .OrderByDescending(b => b.CreatedDate)
                        .Select(b => new BidsResponseList
                        {
                            BidId = b.Id,
                            OfferId = b.OfferId,
                            BidReference = b.BidReference,
                            StartDate = b.Offer.StartDate,
                            EndDate = b.Offer.EndDate,
                            OfferReference = b.Offer.ReferenceNumber,
                            VehicleNumber = b.Offer.Vehicle.RegistrationNumber,
                            Make = b.Offer.Vehicle.Make.Name,
                            Model = b.Offer.Vehicle.Model.Name,
                            PreviousAmount = b.PreviousAmount,
                            SubmittedAmount = b.SubmittedAmount,
                            CustomerName = $"{b.Customer.FirstName} {b.Customer.LastName}",
                            CustomerPhone = b.Customer.Phone,
                            Awarded = b.Awarded,
                            CreatedDate = b.CreatedDate
                        }).ToList()
                })
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (results is null) return null;

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch offers => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<int> ExtendOffer(Guid offerId, string extendedDate)
    {
        try
        {
            var date = DateTime.SpecifyKind(DateTime.ParseExact(extendedDate, "yyyy-MM-dd", null),
                DateTimeKind.Utc);

            var results = await _context.Offers.Where(offer => offer.Id.Equals(offerId))
                .ExecuteUpdateAsync(p => p.SetProperty(d => d.EndDate, date));

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to extend offer => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<int> CompleteOffer(Guid offerId)
    {
        var results = await _context.Offers.Where(offer => offer.Id.Equals(offerId))
            .ExecuteUpdateAsync(p => p.SetProperty(d => d.Status, StatusEnums.Completed));

        // Award Last Highest Bidder

        // Generate their Receipt and Send Email to the Bidder

        return results;
    }

    public async Task<Bid> SubmitBid(BidRequest request)
    {
        try
        {
            var generator = new NumberGenerator(_context);
            var number = generator.GenerateBidReference();

            var newBid = new Bid
            {
                OfferId = request.OfferId,
                CustomerId = request.CustomerId,
                BidReference = number,
                PreviousAmount = request.PreviousAmount,
                SubmittedAmount = request.SubmittedAmount
            };

            await _context.Bids.AddAsync(newBid);
            await _context.SaveChangesAsync();

            await SendOfferNotification(request.OfferId, request.SubmittedAmount);

            return newBid;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to receive bid for the offer => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<Bid?> FetchLastBid(Guid offerId)
    {
        try
        {
            var results = await _context.Bids.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync(x => x.OfferId.Equals(offerId));

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch last bid details => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<BidsResponseList>> FetchAllBids()
    {
        try
        {
            var results = await _context.Bids
                .Select(x => new BidsResponseList
                {
                    BidId = x.Id,
                    OfferId = x.OfferId,
                    OfferReference = x.Offer.ReferenceNumber,
                    BidReference = x.BidReference,
                    StartDate = x.Offer.StartDate,
                    EndDate = x.Offer.EndDate,
                    VehicleNumber = x.Offer.Vehicle.RegistrationNumber,
                    Make = x.Offer.Vehicle.Make.Name,
                    Model = x.Offer.Vehicle.Model.Name,
                    CustomerName = $"{x.Customer.FirstName} {x.Customer.LastName}",
                    CustomerPhone = x.Customer.Phone,
                    PreviousAmount = x.PreviousAmount,
                    SubmittedAmount = x.SubmittedAmount,
                    Awarded = x.Awarded,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch all bids => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<Bid>> FetchOfferBids(Guid offerId)
    {
        try
        {
            // var results = await _context.Bids.Where(x => x.OfferId.Equals(offerId))
            //     .Include(s => s.SystemUser)
            //     .Include(o => o.Offer)
            //     .ToListAsync();
            //
            // return results;

            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch offer bids => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<int> AwardOffer(Guid bidId)
    {
        try
        {
            var result = await _context.Bids.Where(bid => bid.Id.Equals(bidId))
                .ExecuteUpdateAsync(p => p.SetProperty(s => s.Awarded, true));

            // Generate award invoice to be sent to customer email
            await CreateAwardInvoice(bidId);

            return result;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to award bid => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<CustomerOfferResponse>> FetchActiveOffers()
    {
        try
        {
            var offers = await _context.Offers
                .Where(x => x.EndDate <= DateTime.UtcNow.AddDays(2) && x.EndDate >= DateTime.UtcNow)
                .Select(x => new CustomerOfferResponse
                {
                    OfferId = x.Id,
                    ReferenceNumber = x.ReferenceNumber,
                    PlateNumber = x.Vehicle.Reference,
                    VehicleNumber = x.Vehicle.RegistrationNumber,
                    Title = x.Vehicle.Title,
                    Subtitle = x.Vehicle.Description,
                    Year = x.Vehicle.Year,
                    Location = x.Vehicle.Region.RegionName,
                    RegionCode = x.Vehicle.Region.RegionIso,
                    Make = x.Vehicle.Make.Name,
                    Model = x.Vehicle.Model.Name,
                    Reserved = x.Vehicle.Reserved,
                    Status = x.Status.ToString(),
                    InitialPrice = x.ReservePrice,
                    IncrementPrice = x.IncrementPrice,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedDate = x.CreatedDate,
                    Images = x.Vehicle.VehicleImages.Select(img => new ImageResponse
                    {
                        VehicleImageId = img.Id,
                        ImageUrl = img.ImageUrl
                    }).ToList()
                })
                .ToListAsync();

            return offers;
        }
        catch (Exception exception)
        {
            Log.Error("Failed fetch active offers list => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ServiceResponse<List<CustomerOfferResponse>>> FetchEndSoonOffers()
    {
        try
        {
            // Filters offer that end within the next two days 

            var offers = await _context.Offers
                .Where(x => x.EndDate <= DateTime.UtcNow.AddDays(2) && x.EndDate >= DateTime.UtcNow)
                .Select(x => new CustomerOfferResponse
                {
                    OfferId = x.Id,
                    ReferenceNumber = x.ReferenceNumber,
                    PlateNumber = x.Vehicle.Reference,
                    VehicleNumber = x.Vehicle.RegistrationNumber,
                    Title = x.Vehicle.Title,
                    Subtitle = x.Vehicle.Description,
                    Year = x.Vehicle.Year,
                    Location = x.Vehicle.Region.RegionName,
                    RegionCode = x.Vehicle.Region.RegionIso,
                    Make = x.Vehicle.Make.Name,
                    Model = x.Vehicle.Model.Name,
                    Reserved = x.Vehicle.Reserved,
                    Status = x.Status.ToString(),
                    InitialPrice = x.ReservePrice,
                    IncrementPrice = x.IncrementPrice,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedDate = x.CreatedDate,
                    Images = x.Vehicle.VehicleImages.Select(img => new ImageResponse
                    {
                        VehicleImageId = img.Id,
                        ImageUrl = img.ImageUrl
                    }).ToList()
                })
                .ToListAsync();

            return ServiceResponse.Success(offers, "Offers Fetched Successfully");
        }
        catch (Exception exception)
        {
            Log.Error("Failed fetch end soon offers => {@exception}", exception.Message);
            return ServiceResponse.Failure<List<CustomerOfferResponse>>(exception.Message);
        }
    }

    public async Task<ServiceResponse<List<CustomerOfferResponse>>> FetchCompletedOffers()
    {
        try
        {
            // Filters offer that are completed, either by status of end date reached + 1 day

            var offers = await _context.Offers
                .Where(x => x.Status.Equals(StatusEnums.Completed) || x.EndDate >= DateTime.UtcNow.AddDays(1))
                .Select(x => new CustomerOfferResponse
                {
                    OfferId = x.Id,
                    ReferenceNumber = x.ReferenceNumber,
                    PlateNumber = x.Vehicle.Reference,
                    VehicleNumber = x.Vehicle.RegistrationNumber,
                    Title = x.Vehicle.Title,
                    Subtitle = x.Vehicle.Description,
                    Year = x.Vehicle.Year,
                    Location = x.Vehicle.Region.RegionName,
                    RegionCode = x.Vehicle.Region.RegionIso,
                    Make = x.Vehicle.Make.Name,
                    Model = x.Vehicle.Model.Name,
                    Reserved = x.Vehicle.Reserved,
                    Status = x.Status.ToString(),
                    InitialPrice = x.ReservePrice,
                    IncrementPrice = x.IncrementPrice,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedDate = x.CreatedDate,
                    Images = x.Vehicle.VehicleImages.Select(img => new ImageResponse
                    {
                        VehicleImageId = img.Id,
                        ImageUrl = img.ImageUrl
                    }).ToList()
                })
                .ToListAsync();

            return ServiceResponse.Success(offers, "Offers Fetched Successfully");
        }
        catch (Exception exception)
        {
            Log.Error("Failed fetch completed offers => {@exception}", exception.Message);
            return ServiceResponse.Failure<List<CustomerOfferResponse>>(exception.Message);
        }
    }

    public async Task<ActiveOfferResponse?> FetchOfferProfile(Guid offerId)
    {
        try
        {
            var result = await _context.Offers
                .Where(x => x.Id.Equals(offerId))
                .Select(x => new ActiveOfferResponse
                {
                    OfferId = x.Id,
                    ReferenceNumber = x.ReferenceNumber,
                    PlateNumber = x.Vehicle.RegistrationNumber,
                    VehicleNumber = x.Vehicle.Reference,
                    Make = x.Vehicle.Make.Name,
                    Model = x.Vehicle.Model.Name,
                    Mileage = x.Vehicle.Mileage,
                    Reserved = x.Vehicle.Reserved,
                    TitleStatus = x.Vehicle.TitleStatus,
                    Region = x.Vehicle.Region.RegionName,
                    RegionCode = x.Vehicle.Region.RegionIso,
                    Engine = x.Vehicle.Engine,
                    BodyStyle = x.Vehicle.BodyStyle.ToString(),
                    InteriorColor = x.Vehicle.InteriorColor,
                    ExteriorColor = x.Vehicle.ExteriorColor,
                    SellerType = x.Vehicle.Company.Name,
                    Status = x.Status.ToString(),
                    InitialPrice = x.ReservePrice,
                    IncrementPrice = x.IncrementPrice,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreatedDate = x.CreatedDate,
                    TotalBids = x.Bids.Count,
                    LastOfferPrice = x.Bids.OrderByDescending(c => c.CreatedDate).Select(b => b.SubmittedAmount).FirstOrDefault(),
                    Highlights = x.Vehicle.Highlights,
                    Issues = x.Vehicle.Issues,
                    Notes = x.Vehicle.SellerNotes,
                    Images = x.Vehicle.VehicleImages.Select(img => new ImageResponse
                    {
                        VehicleImageId = img.Id,
                        ImageUrl = img.ImageUrl
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }
        catch (Exception exception)
        {
            Log.Error("Failed fetch active offers profile => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<bool> OfferExists(Guid offerId)
    {
        try
        {
            return await _context.Offers.AnyAsync(x => x.Id.Equals(offerId));
        }
        catch (Exception exception)
        {
            Log.Error("Failed fetch active offers profile => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<bool> BidExists(Guid bidId)
    {
        try
        {
            return await _context.Bids.AnyAsync(x => x.Id.Equals(bidId));
        }
        catch (Exception exception)
        {
            Log.Error("Failed check if bid exists => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ServiceResponse<bool>> Subscribe(SubscribeRequest request)
    {
        try
        {
            var newSub = new Subscription
            {
                CustomerId = request.CustomerId,
                OfferId = request.OfferId,
                IsActive = true
            };

            await _context.Subscriptions.AddAsync(newSub);
            await _context.SaveChangesAsync();

            return ServiceResponse.Success(true, "Subscribed to watch offer successfully");
        }
        catch (Exception exception)
        {
            Log.Error("Failed to subscribe to offer => {@exception}", exception);
            return ServiceResponse.Failure<bool>("Failed to subscribe to offer", new List<string> { exception.Message });
        }
    }

    public async Task<ServiceResponse<bool>> Unsubscribe(SubscribeRequest request)
    {
        try
        {
            var sub = await _context.Subscriptions
                .Where(x => x.OfferId.Equals(request.OfferId) && x.CustomerId.Equals(request.CustomerId) && x.IsActive == true)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            if (sub == null) return ServiceResponse.Failure<bool>("Failed to unsubscribe to offer", new List<string> { "OFFER_SUB_NOT_FOUND" });

            sub.IsActive = false;
            sub.UnsubscribedTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ServiceResponse.Success(true, "Unsubscribed from watching offer successfully");
        }
        catch (Exception exception)
        {
            Log.Error("Failed to subscribe to offer => {@exception}", exception);
            return ServiceResponse.Failure<bool>("Failed to unsubscribe to offer", new List<string> { exception.Message });
        }
    }

    public async Task<ServiceResponse<bool>> CheckSubscriptionStatus(CheckSubscription request)
    {
        try
        {
            var sub = await _context.Subscriptions
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync(x => x.CustomerId.Equals(request.CustomerId) && x.OfferId.Equals(request.OfferId) && x.IsActive.Equals(true));

            if (sub == null) return ServiceResponse.Success(false, "No active subscription found");

            return ServiceResponse.Success(true, "Active Subscription Found");
        }
        catch (Exception exception)
        {
            Log.Error("Failed to check sub status => {@exception}", exception);
            return ServiceResponse.Failure<bool>("Failed to check sub status", new List<string> { exception.Message });
        }
    }

    public async Task<ServiceResponse<OfferStatisticsResponse>> FetchStatistics()
    {
        try
        {
            var statistics = new OfferStatisticsResponse
            {
                TotalOffersCompleted = await _context.Offers.CountAsync(
                    offer => offer.Status == StatusEnums.Completed),
                VehiclesAvailable = await _context.Vehicles.CountAsync(
                    vehicle => !vehicle.Reserved),
                CustomersRegistered = await _context.Customers.CountAsync(),
                TotalSuccessfulBids = await _context.Bids.CountAsync(
                    bid => bid.Awarded)
            };

            return ServiceResponse.Success(statistics, "Offer statistics fetched successfully");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to fetch offer statistics");
            return ServiceResponse.Failure<OfferStatisticsResponse>(
                "Failed to fetch offer statistics",
                new List<string> { exception.Message });
        }
    }

    private async Task SendOfferNotification(Guid offerId, decimal amount)
    {
        try
        {
            var vehicle = await _context.Offers
                .Where(o => o.Id.Equals(offerId))
                .Select(x => new
                {
                    Vehicle = $"{x.Vehicle.RegistrationNumber} {x.Vehicle.Make.Name} {x.Vehicle.Model.Name}"
                })
                .FirstOrDefaultAsync();

            var subs = await _context.Subscriptions
                .Include(x => x.Customer)
                .Where(x => x.OfferId.Equals(offerId) && x.IsActive && !string.IsNullOrEmpty(x.Customer.Phone))
                .Select(x => new
                {
                    x.Customer.Phone,
                    Name = $"{x.Customer.FirstName} {x.Customer.LastName}"
                })
                .ToListAsync();

            var formattedAmount = NumberGenerator.FormatCurrency(amount);

            var notificationRequest = new BulkOfferNotify
            {
                Messages = subs.Select(r => new BidMessage
                {
                    From = "SMATSALVAGE",
                    To = r.Phone,
                    Text = $"Dear {r.Name}. New Offer placed for vehicle {vehicle?.Vehicle} Current Offer: {formattedAmount}. Hurry to https://smartsalvagetz.com/offers/{offerId} and Place your offer"
                }).ToList(),
                Flash = 0,
                Reference = offerId.ToString("N").Substring(0, 8)
            };

            Log.Information("Sending offer notification notification => {@notificationRequest}", notificationRequest);
            var smsJob = BackgroundJob.Enqueue<INotificationService>(x => x.SendBidNotifications(notificationRequest));
            Log.Information("Bid Notification Job Created => {@job}", smsJob);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send offer notifications => {@exception}", exception);
            throw;
        }
    }

    private async Task<InsertResponse> CreateAwardInvoice(Guid bidId)
    {
        try
        {
            var bid = await _context.Bids
                .Where(bid => bid.Id.Equals(bidId))
                .Select(x => new BidResponse
                {
                    BidId = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = $"{x.Customer.FirstName} {x.Customer.LastName}",
                    CustomerPhone = x.Customer.Phone,
                    BidReference = x.BidReference,
                    SubmittedAmount = x.SubmittedAmount,
                    VehicleNumber = x.Offer.Vehicle.RegistrationNumber,
                    Make = x.Offer.Vehicle.Make.Name,
                    Model = x.Offer.Vehicle.Model.Name
                })
                .FirstOrDefaultAsync();

            if (bid is null) throw new Exception("Bid not found");

            var number = InvoiceNumberGenerator.GenerateTimeBasedInvoice();

            var invoice = new Invoice
            {
                Reference = number,
                CustomerId = bid.CustomerId,
                BidId = bid.BidId,
                Amount = bid.SubmittedAmount
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            var response = new InsertResponse
            {
                IsSuccess = true,
                Result = invoice
            };

            if (response.IsSuccess)
            {
                // Generate PDF Invoice and Save file path
                var filePath = await _pdfService.GenerateCustomerInvoice(invoice);

                // Send Award Email and Create Notification

                // Send SMS Notification to Customer notifying them that they have won
                await NotifyCustomer(bid);
            }

            return response;
        }
        catch (Exception exception)
        {
            var response = new InsertResponse
            {
                IsSuccess = false,
                Result = exception
            };
            Log.Error("Failed to generate award invoice  => {@exception}", exception);
            return response;
        }
    }

    private async Task NotifyCustomer(BidResponse bid)
    {
        try
        {
            var senderId = _configuration.GetValue<string>("CustomSettings:SenderId");
            var campaignId = _configuration.GetValue<string>("CustomSettings:CampaignId");

            var today = DateTime.UtcNow.AddHours(48);

            var contact = "+255778009990";

            var message =
                $"Congratulations! {bid.CustomerName} You have won the bid for vehicle {bid.Make} {bid.Model} with Plate No. {bid.VehicleNumber}. Please complete your payment of TZS {bid.SubmittedAmount} and other arrangements by {today}. For more information, contact us at {contact}.";

            ICollection<string> recipients = new List<string> { bid.CustomerPhone };

            var payload = new NotificationPayload
            {
                Content = message,
                Recipients = recipients,
                SenderId = senderId,
                CampaignId = campaignId
            };

            var jobId = BackgroundJob.Enqueue<INotificationService>(x => x.SendAwardNotification(payload));

            Log.Information("Posting SMS Job Id {@payload}", jobId);
            await Task.CompletedTask;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to notify user on winning award  => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }
}
