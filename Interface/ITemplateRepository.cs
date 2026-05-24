using SalvageCore.DTOs.Template.Request;
using SalvageCore.DTOs.Template.Response;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface ITemplateRepository
{
    public Task<Template> CreateTemplate(CreateTemplateReq request);
    public Task<List<TemplateListResponse>> FetchTemplateList();
    public Task<TemplateListResponse> FetchTemplateProfile(Guid id);
    public Task<bool> TemplateExists(Guid id);
    public Task<Template> EditTemplate();
    public Task<Template> DeleteTemplate(Guid id);
}