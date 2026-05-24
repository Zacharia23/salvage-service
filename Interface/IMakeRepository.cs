using SalvageCore.DTOs.Make.Request;
using SalvageCore.DTOs.Make.Response;
using SalvageCore.Helpers;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface IMakeRepository
{
    public Task<Make> CreateVehicleMake(MakeRequest request);
    public Task<ServiceResponse<Make>> UpdateVehicleMake(MakeRequest request);
    public Task<ServiceResponse<Model>> AddVehicleModel(AddModelRequest model);
    public Task<ServiceResponse<Model>> UpdateVehicleModel(Model model);
    public Task<ServiceResponse<Model>> UpdateMakeModel(Make model);
    public Task<ICollection<MakeResponse>> FetchVehicleMakes();
}