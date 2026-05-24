using Microsoft.AspNetCore.Identity;
using SalvageCore.DTOs.Company.Response;
using SalvageCore.DTOs.Customer.Response;
using SalvageCore.DTOs.Region.Response;
using SalvageCore.DTOs.Vehicle.Response;
using MakeResponse = SalvageCore.DTOs.Make.Response.MakeResponse;

namespace SalvageCore.Models;

public class SplashModel
{
    public ICollection<CompanyTypes> CompanyTypes { get; set; } = new List<CompanyTypes>();
    public ICollection<MakeResponse> Makes { get; set; } = new List<MakeResponse>();
    public ICollection<RegionResponse> Regions { get; set; } = new List<RegionResponse>();
    public ICollection<EnumResponse> Drives { get; set; } = new List<EnumResponse>();
    public ICollection<EnumResponse> Transmission { get; set; } = new List<EnumResponse>();
    public ICollection<EnumResponse> BodyStyle { get; set; } = new List<EnumResponse>();
    public ICollection<CompanySplashResponse> Companies { get; set; } = new List<CompanySplashResponse>();
    public ICollection<EnumResponse> OfferTypes { get; set; } = new List<EnumResponse>();
    public ICollection<VehicleMiniResponseList> Vehicles { get; set; } = new List<VehicleMiniResponseList>();
    public ICollection<IdTypeList> IDTypes { get; set; } = new List<IdTypeList>();
    public ICollection<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
    public ICollection<EnumResponse> Gender { get; set; } = new List<EnumResponse>();
    public ICollection<EnumResponse> Account { get; set; } = new List<EnumResponse>();
    public ICollection<EnumResponse> Eligibility { get; set; } = new List<EnumResponse>();
}