using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Company.Request;
using SalvageCore.DTOs.Company.Response;
using SalvageCore.DTOs.Region.Response;
using SalvageCore.DTOs.Vehicle.Response;
using SalvageCore.Enums;
using SalvageCore.Interface;
using SalvageCore.Models;
using SalvageCore.Service;
using Serilog;

namespace SalvageCore.Repository;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<CompanyResponse>> FetchCompanies()
    {
        try
        {
            var results = await _context.Companies
                .Select(x => new CompanyResponse
                {
                    CompanyId = x.Id,
                    CompanyType = x.CompanyType.ToString(),
                    Number = x.Number,
                    CompanyName = x.Name,
                    ContactPerson = x.ContactPerson,
                    Phone = x.Phone,
                    Email = x.Email,
                    Location = x.Location,
                    CreatedDate = x.CreatedDate,
                    CompanyLogo = x.LogoUrl,
                    TotalVehicles = x.Vehicles.Count(),
                    Status = x.Status.ToString()
                })
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch companies => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<Company> CreateCompany(CompanyRequest company, string imageUrl)
    {
        try
        {
            var number = NumberGenerator.GenerateCompanyNumber();

            var newCompany = new Company
            {
                Name = company.CompanyName,
                Number = number,
                ContactPerson = company.ContactPerson,
                Phone = company.Phone,
                Email = company.Email,
                Location = company.Location,
                Status = StatusEnums.Active,
                CompanyType = company.CompanyType,
                LogoUrl = imageUrl
            };

            await _context.AddAsync(newCompany);
            await _context.SaveChangesAsync();

            return newCompany;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Create Company => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<CompanyDetailsResponse> FetchCompanyDetails(Guid companyId)
    {
        try
        {
            var results = await _context.Companies
                .Where(c => c.Id.Equals(companyId))
                .Select(x => new CompanyDetailsResponse
                {
                    CompanyId = x.Id,
                    CompanyType = x.CompanyType.ToString(),
                    Number = x.Number,
                    CompanyName = x.Name,
                    ContactPerson = x.ContactPerson,
                    Phone = x.Phone,
                    Email = x.Email,
                    Location = x.Location,
                    CreatedDate = x.CreatedDate,
                    Status = x.Status.ToString(),
                    CompanyLogo = x.LogoUrl,
                    TotalVehicles = x.Vehicles.Count(),
                    Vehicles = x.Vehicles.Select(v => new VehicleResponseList
                    {
                        VehicleId = v.Id,
                        Title = v.Title,
                        Subtitle = v.Description,
                        Reserved = v.Reserved,
                        Make = v.Make.Name,
                        Model = v.Model.Name,
                        Year = v.Year,
                        Mileage = v.Mileage,
                        Engine = v.Engine,
                        TitleStatus = v.TitleStatus,
                        Region = v.Region.RegionName,
                        BodyStyle = v.BodyStyle.ToString(),
                        ExteriorColor = v.ExteriorColor,
                        InteriorColor = v.InteriorColor,
                        Highlights = v.Highlights,
                        Issues = v.Issues,
                        LastService = v.LastService,
                        SellerNotes = v.SellerNotes,
                        CreatedDate = v.CreatedDate
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Company Details => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<int> UpdateCompany(Guid companyId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<bool> CompanyExists(Guid companyId)
    {
        try
        {
            return await _context.Companies.AnyAsync(c => c.Id.Equals(companyId));
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Company Existence => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<RegionResponse>> FetchRegion()
    {
        try
        {
            var results = await _context.Regions
                .Select(x => new RegionResponse
                {
                    RegionId = x.Id,
                    RegionIso = x.RegionIso,
                    RegionName = x.RegionName,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Regions => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<CompanySplashResponse>> FetchSplashCompanies()
    {
        try
        {
            var results = await _context.Companies
                .Select(x => new CompanySplashResponse
                {
                    CompanyId = x.Id,
                    CompanyName = x.Name
                })
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Fetch Regions => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }
}