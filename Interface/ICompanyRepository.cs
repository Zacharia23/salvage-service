using SalvageCore.DTOs.Company.Request;
using SalvageCore.DTOs.Company.Response;
using SalvageCore.DTOs.Region.Response;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface ICompanyRepository
{
    public Task<ICollection<CompanyResponse>> FetchCompanies();
    public Task<Company> CreateCompany(CompanyRequest company, string imageUrl);
    public Task<CompanyDetailsResponse> FetchCompanyDetails(Guid companyId);
    public Task<int> UpdateCompany(Guid companyId);
    public Task<bool> CompanyExists(Guid companyId);
    public Task<ICollection<RegionResponse>> FetchRegion();
    public Task<ICollection<CompanySplashResponse>> FetchSplashCompanies();
}