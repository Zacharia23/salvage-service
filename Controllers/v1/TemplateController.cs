using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalvageCore.DTOs.Template.Request;
using SalvageCore.Extensions;
using SalvageCore.Interface;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class TemplateController : ControllerBase
{
    private readonly ITemplateRepository _templateRepository;

    public TemplateController(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    [HttpPost]
    [Route("[Controller]/CreateTemplate")]
    public async Task<IActionResult> CreateTemplateTask([FromBody] CreateTemplateReq request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _templateRepository.CreateTemplate(request);

        return this.Respond("Template Created Successfully", result.Name);
    }

    [HttpGet]
    [Route("[Controller]/FetchTemplates")]
    public async Task<IActionResult> FetchTemplateList()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _templateRepository.FetchTemplateList();

        return this.Respond("Templates Fetched Successfully", result);
    }

    [HttpGet]
    [Route("[Controller]/FetchTemplateProfile/{id:guid}")]
    public async Task<IActionResult> FetchTemplateProfile([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _templateRepository.TemplateExists(id)) return this.RespondNotFound($"Template with id {id} not found");

        var result = await _templateRepository.FetchTemplateProfile(id);

        return this.Respond("Template Profile Fetched Successfully", result);
    }
}