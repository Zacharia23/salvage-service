using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalvageCore.DTOs.Vehicle.Request;
using SalvageCore.Extensions;
using SalvageCore.Interface;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class VehicleController : ControllerBase
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUploadService _uploadService;
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleController(IVehicleRepository vehicleRepository, IUploadService uploadService, ICompanyRepository companyRepository)
    {
        _vehicleRepository = vehicleRepository;
        _uploadService = uploadService;
        _companyRepository = companyRepository;
    }

    [HttpPost]
    [Route("[Controller]/CreateVehicle")]
    public async Task<IActionResult> RegisterVehicle([FromForm] VehicleRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);
        Log.Information("Registered Vehicle Payload {@Request}", request);

        Log.Information("Images Sent Count {@images}", request.Images.Count);

        if (request.Images.Count == 0) return this.RespondError(StatusCodes.Status400BadRequest, "Select at least one or more images");

        ICollection<string> uploadedPaths = new List<string>();

        foreach (var image in request.Images)
        {
            Log.Information("Uploading image {@Image}", image.FileName);

            if (image.Length == 0)
                //Skips Files with no content
                continue;

            await using (var fileStream = image.OpenReadStream())
            {
                var uploadResponse = await _uploadService.UploadVehicleImages(fileStream, image.FileName);
                if (uploadResponse.IsSuccess)
                    uploadedPaths.Add(uploadResponse.ImageUrl);
                else
                    return this.RespondError(StatusCodes.Status500InternalServerError,
                        $"Failed to upload image: {image.FileName}");
            }
        }

        var result = await _vehicleRepository.CreateVehicle(request, uploadedPaths.ToList());

        return this.Respond("Vehicle Created Successfully", result.RegistrationNumber);
    }

    [HttpGet]
    [Route("[Controller]/FetchVehicles")]
    public async Task<IActionResult> FetchVehicles()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var vehicles = await _vehicleRepository.FetchVehicles();

        return this.Respond("Vehicles Fetched Successfully", vehicles);
    }

    [HttpGet]
    [Route("[Controller]/FetchCompanyVehicles/{companyId:guid}")]
    public async Task<IActionResult> FetchCompanyVehicles([FromRoute] Guid companyId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _companyRepository.CompanyExists(companyId)) return this.RespondNotFound($"Company {companyId} Not Found");
        var vehicles = await _vehicleRepository.FetchCompanyVehicles(companyId);

        return this.Respond("Company Vehicles Fetched Successfully", vehicles);
    }

    [HttpGet]
    [Route("[Controller]/FetchVehicleProfile/{vehicleId:guid}")]
    public async Task<IActionResult> FetchVehicleDetails([FromRoute] Guid vehicleId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _vehicleRepository.VehicleExists(vehicleId)) return this.RespondNotFound($"Vehicle {vehicleId} Not Found");

        var vehicle = await _vehicleRepository.FetchVehicleDetails(vehicleId);

        return this.Respond("Vehicle Details Fetched Successfully", vehicle);
    }

    [HttpPost]
    [Route("[Controller]/UpdateVehicleImages")]
    public async Task<IActionResult> UploadVehicleImages([FromForm] FileModel model)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _vehicleRepository.VehicleExists(model.VehicleId)) return this.RespondNotFound($"Vehicle {model.VehicleId} Not Found");

        return this.Respond("Vehicle Images Uploaded Successfully", "uploadedFilePaths");
    }
}