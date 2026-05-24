using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalvageCore.DTOs.Company.Request;
using SalvageCore.Extensions;
using SalvageCore.Interface;
using SalvageCore.Models;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyRepository _company;
    private readonly IUploadService _uploadService;

    public CompanyController(ICompanyRepository company, IUploadService uploadService)
    {
        _company = company;
        _uploadService = uploadService;
    }

    [HttpPost]
    [Route("[Controller]/CreateCompany")]
    public async Task<IActionResult> RegisterCompanyAction([FromForm] CompanyRequest company)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        // Upload Image First
        UploadResponse response;

        await using (var fileStream = company.File.OpenReadStream())
        {
            response = await _uploadService.UploadCompanyLogo(fileStream, company.File.FileName);
        }

        if (!response.IsSuccess) return this.RespondError(StatusCodes.Status400BadRequest, "Failed to upload company log");

        var results = await _company.CreateCompany(company, response.ImageUrl);

        return this.Respond("Company Created Successfully", results.Name);
    }

    [HttpGet]
    [Route("[Controller]/FetchCompanies")]
    public async Task<IActionResult> FetchCompaniesAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var companies = await _company.FetchCompanies();

        return this.Respond("Companies Fetched Successfully", companies);
    }

    [HttpGet]
    [Route("[Controller]/FetchCompanyProfile/{companyId:guid}")]
    public async Task<IActionResult> FetchCompanyDetailsAction([FromRoute] Guid companyId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _company.CompanyExists(companyId)) return this.RespondNotFound($"Company with id {companyId} not found");

        var company = await _company.FetchCompanyDetails(companyId);

        return this.Respond("Company Details Fetched Successfully", company);
    }
}