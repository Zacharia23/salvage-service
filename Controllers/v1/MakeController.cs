using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalvageCore.DTOs.Make.Request;
using SalvageCore.Extensions;
using SalvageCore.Interface;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class MakeController : ControllerBase
{
    private readonly IMakeRepository _makeRepository;

    public MakeController(IMakeRepository makeRepository)
    {
        _makeRepository = makeRepository;
    }

    [HttpPost]
    [Route("[Controller]/CreateVehicleMake")]
    public async Task<IActionResult> CreateMakeAction([FromBody] MakeRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var response = await _makeRepository.CreateVehicleMake(request);
        return this.Respond("Vehicle Make Added Successfully", response.Name);
    }

    [HttpGet]
    [Route("[Controller]/FetchVehicleMakes")]
    public async Task<IActionResult> FetchMakesAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var response = await _makeRepository.FetchVehicleMakes();

        return this.Respond("Vehicle Models Fetched Successfully", response);
    }

    [HttpPost]
    [Route("[Controller]/CreateModel")]
    public async Task<IActionResult> CreateMakeAction([FromBody] AddModelRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var response = await _makeRepository.AddVehicleModel(request);

        if (!response.Success) return this.RespondError(StatusCodes.Status400BadRequest, response.Errors.ToString());

        return this.Respond("Model Created Successfully", response.Data);
    }
}