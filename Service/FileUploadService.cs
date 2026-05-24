using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SalvageCore.Interface;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Service;

public class FileUploadService : IUploadService
{
    private readonly IConfiguration _configuration;

    public FileUploadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<UploadResponse> UploadVehicleImages(Stream fileStream, string fileName)
    {
        try
        {
            var cloudinaryUrl = _configuration.GetValue<string>("CustomSettings:CloudinaryUrl");
            var cloudinary = new Cloudinary(cloudinaryUrl)
            {
                Api = { Secure = true }
            };

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = fileName,
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = true,
                Folder = "Vehicles"
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            var url = uploadResult.JsonObj["secure_url"];

            var result = new UploadResponse
            {
                IsSuccess = true,
                ImageUrl = url.ToString()
            };

            return result;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to upload vehicle images => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<UploadResponse> UploadCompanyLogo(Stream fileStream, string fileName)
    {
        try
        {
            var cloudinaryUrl = _configuration.GetValue<string>("CustomSettings:CloudinaryUrl");
            var cloudinary = new Cloudinary(cloudinaryUrl)
            {
                Api = { Secure = true }
            };

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                PublicId = fileName,
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = true,
                Folder = "Vehicles"
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            var url = uploadResult.JsonObj["secure_url"];

            var result = new UploadResponse
            {
                IsSuccess = true,
                ImageUrl = url.ToString()
            };

            return result;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to upload company logo => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }
}