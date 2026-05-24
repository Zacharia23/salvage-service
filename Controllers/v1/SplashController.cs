using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalvageCore.DTOs.Company.Response;
using SalvageCore.Enums;
using SalvageCore.Extensions;
using SalvageCore.Interface;
using SalvageCore.Models;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class SplashController : ControllerBase
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMakeRepository _makeRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IVehicleRepository _vehicleRepository;

    public SplashController(ICompanyRepository companyRepository, IMakeRepository makeRepository, IVehicleRepository vehicleRepository, IOfferRepository offerRepository,
        ICustomerRepository customerRepository, RoleManager<IdentityRole> roleManager)
    {
        _companyRepository = companyRepository;
        _makeRepository = makeRepository;
        _vehicleRepository = vehicleRepository;
        _offerRepository = offerRepository;
        _customerRepository = customerRepository;
        _roleManager = roleManager;
    }

    [HttpPost]
    [Route("FetchSplash")]
    public async Task<IActionResult> FetchSplashAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var companyTypes = Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().Select(type => new CompanyTypes
        {
            Name = type.ToString(),
            Value = (int)type
        }).ToList();

        var drives = Enum.GetValues(typeof(DriveEnum)).Cast<DriveEnum>().Select(drive => new EnumResponse
        {
            Name = drive.ToString(),
            Value = (int)drive
        }).ToList();

        var body = Enum.GetValues(typeof(BodyStyle)).Cast<BodyStyle>().Select(body => new EnumResponse
        {
            Name = body.ToString(),
            Value = (int)body
        }).ToList();

        var transmission = Enum.GetValues(typeof(TransmissionEnum)).Cast<TransmissionEnum>().Select(trans =>
            new EnumResponse
            {
                Name = trans.ToString(),
                Value = (int)trans
            }).ToList();

        var offerTypes = Enum.GetValues(typeof(NatureEnum)).Cast<NatureEnum>().Select(nature => new EnumResponse
        {
            Name = nature.ToString(),
            Value = (int)nature
        }).ToList();

        var gender = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().Select(nature => new EnumResponse
        {
            Name = nature.ToString(),
            Value = (int)nature
        }).ToList();

        var accountType = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().Select(nature => new EnumResponse
        {
            Name = nature.ToString(),
            Value = (int)nature
        }).ToList();

        var eligibility = Enum.GetValues(typeof(Eligibility)).Cast<Eligibility>().Select(nature => new EnumResponse
        {
            Name = nature.ToString(),
            Value = (int)nature
        }).ToList();

        var vehicleMakes = await _makeRepository.FetchVehicleMakes();
        var regions = await _companyRepository.FetchRegion();
        var companies = await _companyRepository.FetchSplashCompanies();
        var vehicles = await _vehicleRepository.FetchVehicleMiniList();
        var idTypes = await _customerRepository.FetchIdentityTypes();
        var roles = await _roleManager.Roles.ToListAsync();

        var splash = new SplashModel
        {
            CompanyTypes = companyTypes,
            Makes = vehicleMakes,
            Regions = regions,
            Companies = companies,
            Vehicles = vehicles,
            Drives = drives,
            BodyStyle = body,
            Transmission = transmission,
            OfferTypes = offerTypes,
            IDTypes = idTypes,
            Roles = roles,
            Gender = gender,
            Account = accountType,
            Eligibility = eligibility
        };

        return this.Respond("Splash Fetched Successfully", splash);
    }
}