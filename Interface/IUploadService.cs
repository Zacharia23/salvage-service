using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface IUploadService
{
    Task<UploadResponse> UploadVehicleImages(Stream fileStream, string fileName);
    Task<UploadResponse> UploadCompanyLogo(Stream fileStream, string fileName);
}