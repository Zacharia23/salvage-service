using SalvageCore.DTOs.Vehicle.Request;
using SalvageCore.DTOs.Vehicle.Response;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface IVehicleRepository
{
    public Task<Vehicle> CreateVehicle(VehicleRequest request, List<string> uploadedPaths);
    public Task<Vehicle> EditVehicle(Guid vehicleId);
    public Task<Vehicle> UpdateVehicleStatus(Guid vehicleId);
    public Task<ICollection<VehicleResponseList>> FetchVehicles();
    public Task<ICollection<VehicleResponseList>> FetchCompanyVehicles(Guid vehicleId);
    public Task<VehicleDetailsResponse> FetchVehicleDetails(Guid vehicleId);
    public Task<bool> VehicleExists(Guid vehicleId);
    public Task<VehicleImage> CreateVehicleImage(string imageUrl, Guid vehicleId);
    public Task<ICollection<VehicleMiniResponseList>> FetchVehicleMiniList();
}