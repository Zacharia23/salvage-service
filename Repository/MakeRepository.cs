using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Make.Request;
using SalvageCore.DTOs.Make.Response;
using SalvageCore.Helpers;
using SalvageCore.Interface;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Repository;

public class MakeRepository(ApplicationDbContext context) : IMakeRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Make> CreateVehicleMake(MakeRequest request)
    {
        try
        {
            var results = new Make
            {
                Name = request.Name,
                Models = request.Models.Select(m => new Model
                {
                    Name = m.Name
                }).ToList()
            };

            await _context.Makes.AddAsync(results);
            await _context.SaveChangesAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to create Vehicle Make => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ServiceResponse<Make>> UpdateVehicleMake(MakeRequest request)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Model>> AddVehicleModel(AddModelRequest model)
    {
        try
        {
            var makeExists = await _context.Makes.AnyAsync(m => m.Id.Equals(model.MakeId));

            if (!makeExists) return ServiceResponse.Failure<Model>("Failed to add Vehicle Model", new List<string?> { $"Make with {model.MakeId} does not exist" });

            var modelResponse = new Model
            {
                Name = model.Name,
                MakeId = model.MakeId
            };

            await _context.Models.AddAsync(modelResponse);
            await _context.SaveChangesAsync();

            return ServiceResponse.Success(modelResponse, $"Vehicle Model {modelResponse.Name} Added Successfully");
        }
        catch (Exception exception)
        {
            Log.Information("Failed to add Vehicle Model {@exception}", exception.Message);
            return ServiceResponse.Failure<Model>("Failed to add Vehicle Model", new List<string?> { exception.Message });
        }
    }

    public async Task<ServiceResponse<Model>> UpdateVehicleModel(Model model)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Model>> UpdateMakeModel(Make model)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<ICollection<MakeResponse>> FetchVehicleMakes()
    {
        try
        {
            var results = await _context.Makes
                .OrderByDescending(d => d.CreatedDate)
                .Select(x => new MakeResponse
                {
                    MakeId = x.Id,
                    Name = x.Name,
                    Models = x.Models.Select(m => new ModelResponse
                    {
                        ModelId = m.Id,
                        Name = m.Name
                    }).ToList()
                })
                .ToListAsync();

            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch vehicle makes => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }
}