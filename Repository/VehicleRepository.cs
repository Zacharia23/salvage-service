using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Offer.Response;
using SalvageCore.DTOs.Vehicle.Request;
using SalvageCore.DTOs.Vehicle.Response;
using SalvageCore.Interface;
using SalvageCore.Models;
using SalvageCore.Service;
using Serilog;

namespace SalvageCore.Repository;

public class VehicleRepository(ApplicationDbContext context) : IVehicleRepository
{
    public async Task<Vehicle> CreateVehicle(VehicleRequest request, List<string> uploadedPaths)
    {
        try
        {
            var referenceNumber = NumberGenerator.GenerateVehicleReference();

            var newVehicle = new Vehicle
            {
                CompanyId = request.CompanyId,
                Title = request.Title,
                Description = request.Description,
                MakeId = request.MakeId,
                ModelId = request.ModelId,
                Year = request.Year,
                Mileage = request.Mileage,
                RegistrationNumber = request.RegistrationNumber,
                Reference = referenceNumber,
                Engine = request.Engine,
                TitleStatus = request.TitleStatus,
                RegionId = request.RegionId,
                BodyStyle = request.BodyStyle,
                ExteriorColor = request.ExteriorColor,
                InteriorColor = request.InteriorColor,
                Highlights = request.Highlights,
                Issues = request.Issues,
                LastService = DateTime.UtcNow,
                SellerNotes = request.SellerNotes,
                VehicleImages = uploadedPaths.Select(path => new VehicleImage
                {
                    ImageUrl = path
                }).ToList()
            };

            await context.Vehicles.AddAsync(newVehicle);
            await context.SaveChangesAsync();

            return newVehicle;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to create Vehicle Model => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<Vehicle> EditVehicle(Guid vehicleId)
    {
        try
        {
            var result = new Vehicle();

            await context.SaveChangesAsync();

            return result;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Edit Vehicle => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<Vehicle> UpdateVehicleStatus(Guid vehicleId)
    {
        try
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Update Vehicle Status => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<VehicleResponseList>> FetchVehicles()
    {
        try
        {
            var results = await context
                .Vehicles
                .Select(x => new VehicleResponseList
                {
                    VehicleId = x.Id,
                    CompanyId = x.CompanyId,
                    CompanyName = x.Company.Name,
                    Title = x.Title,
                    ReferenceNumber = x.Reference,
                    PlateNumber = x.RegistrationNumber,
                    Subtitle = x.Description,
                    Reserved = x.Reserved,
                    Make = x.Make.Name,
                    Model = x.Model.Name,
                    Year = x.Year,
                    Mileage = x.Mileage,
                    Engine = x.Engine,
                    TitleStatus = x.TitleStatus,
                    Region = x.Region.RegionName,
                    Drive = x.Drive.ToString(),
                    Transmission = x.Transmission.ToString(),
                    BodyStyle = x.BodyStyle.ToString(),
                    ExteriorColor = x.ExteriorColor,
                    InteriorColor = x.InteriorColor,
                    Highlights = x.Highlights,
                    Issues = x.Issues,
                    LastService = x.LastService,
                    SellerNotes = x.SellerNotes,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Vehicles => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<VehicleResponseList>> FetchCompanyVehicles(Guid companyId)
    {
        try
        {
            var results = await context
                .Vehicles
                .Where(c => c.CompanyId.Equals(companyId))
                .Select(x => new VehicleResponseList
                {
                    VehicleId = x.Id,
                    CompanyId = x.CompanyId,
                    CompanyName = x.Company.Name,
                    Title = x.Title,
                    Subtitle = x.Description,
                    PlateNumber = x.RegistrationNumber,
                    ReferenceNumber = x.Reference,
                    Reserved = x.Reserved,
                    Make = x.Make.Name,
                    Model = x.Model.Name,
                    Year = x.Year,
                    Mileage = x.Mileage,
                    Engine = x.Engine,
                    TitleStatus = x.TitleStatus,
                    Region = x.Region.RegionName,
                    BodyStyle = x.BodyStyle.ToString(),
                    ExteriorColor = x.ExteriorColor,
                    InteriorColor = x.InteriorColor,
                    Highlights = x.Highlights,
                    Issues = x.Issues,
                    LastService = x.LastService,
                    SellerNotes = x.SellerNotes,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Company Vehicles => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<VehicleDetailsResponse> FetchVehicleDetails(Guid vehicleId)
    {
        try
        {
            var results = await context
                .Vehicles
                .Where(c => c.Id.Equals(vehicleId))
                .Select(x => new VehicleDetailsResponse
                {
                    VehicleId = x.Id,
                    CompanyId = x.CompanyId,
                    ReferenceNumber = x.Reference,
                    PlateNumber = x.RegistrationNumber,
                    CompanyName = x.Company.Name,
                    Title = x.Title,
                    Reserved = x.Reserved,
                    Make = x.Make.Name,
                    Model = x.Model.Name,
                    Year = x.Year,
                    Mileage = x.Mileage,
                    Engine = x.Engine,
                    TitleStatus = x.TitleStatus,
                    Region = x.Region.RegionName,
                    ExteriorColor = x.ExteriorColor,
                    InteriorColor = x.InteriorColor,
                    Highlights = x.Highlights,
                    Issues = x.Issues,
                    LastService = x.LastService,
                    SellerNotes = x.SellerNotes,
                    CreatedDate = x.CreatedDate,
                    VehicleImages = x.VehicleImages.Select(vi => new ImageResponse
                    {
                        VehicleImageId = vi.Id,
                        ImageUrl = vi.ImageUrl
                    }).ToList(),
                    Offer = x.Offer != null
                        ? new OfferDetailsResponse
                        {
                            OfferId = x.Offer.Id,
                            ReferenceNumber = x.Offer.ReferenceNumber,
                            InitialPrice = x.Offer.ReservePrice,
                            IncrementPrice = x.Offer.IncrementPrice,
                            StartDate = x.Offer.StartDate,
                            EndDate = x.Offer.EndDate,
                            CreatedDate = x.CreatedDate,
                            Bids = x.Offer.Bids.Select(b => new BidsResponseList
                            {
                                BidId = b.Id,
                                OfferId = b.OfferId,
                                BidReference = b.BidReference,
                                StartDate = x.Offer.StartDate,
                                EndDate = x.Offer.EndDate,
                                OfferReference = x.Offer.ReferenceNumber,
                                VehicleNumber = x.RegistrationNumber,
                                Make = x.Make.Name,
                                Model = x.Model.Name,
                                PreviousAmount = b.PreviousAmount,
                                SubmittedAmount = b.SubmittedAmount,
                                Awarded = b.Awarded,
                                CreatedDate = b.CreatedDate
                            }).ToList()
                        }
                        : null
                }).FirstOrDefaultAsync();

            if (results is null) return null;

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Vehicle Details => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<bool> VehicleExists(Guid vehicleId)
    {
        try
        {
            var results = await context.Vehicles.AnyAsync(id => id.Id.Equals(vehicleId));

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Find Vehicle => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<VehicleImage> CreateVehicleImage(string imageUrl, Guid vehicleId)
    {
        try
        {
            var vehicleImages = new VehicleImage
            {
                VehicleId = vehicleId,
                ImageUrl = imageUrl
            };

            await context.AddAsync(vehicleImages);
            await context.SaveChangesAsync();

            return vehicleImages;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to create vehicle images => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<VehicleMiniResponseList>> FetchVehicleMiniList()
    {
        try
        {
            var results = await context
                .Vehicles
                .Select(x => new VehicleMiniResponseList
                {
                    VehicleId = x.Id,
                    ReferenceNumber = x.Reference,
                    PlateNumber = x.RegistrationNumber,
                    Make = x.Make.Name,
                    Model = x.Model.Name
                })
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Vehicles => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }
}