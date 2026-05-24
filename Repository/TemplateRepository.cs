using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Template.Request;
using SalvageCore.DTOs.Template.Response;
using SalvageCore.Interface;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Repository;

public class TemplateRepository : ITemplateRepository
{
    private readonly ApplicationDbContext _context;

    public TemplateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Template> CreateTemplate(CreateTemplateReq request)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var template = new Template
                {
                    Name = request.Name,
                    Channel = request.Channel,
                    Content = request.Content,
                    Subject = request.Subject,
                    IsActive = true
                };

                await _context.Templates.AddAsync(template);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return template;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                Log.Error("Failed to create template => {@exception}", exception);
                throw new InvalidOperationException("Failed to create template", exception);
            }
        }
    }

    public async Task<List<TemplateListResponse>> FetchTemplateList()
    {
        try
        {
            var response = await _context.Templates
                .Select(x => new TemplateListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Channel = x.Channel.ToString(),
                    Content = x.Content,
                    Subject = x.Subject,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return response;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch template list => {@exception}", exception);
            throw new InvalidOperationException("Failed to fetch template list", exception);
        }
    }

    public async Task<TemplateListResponse> FetchTemplateProfile(Guid id)
    {
        try
        {
            var response = await _context.Templates
                .Where(x => x.Id.Equals(id))
                .Select(x => new TemplateListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Channel = x.Channel.ToString(),
                    Content = x.Content,
                    Subject = x.Subject,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();

            return response;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch template profile => {@exception}", exception);
            throw new InvalidOperationException("Template profile not found", exception);
        }
    }

    public async Task<bool> TemplateExists(Guid id)
    {
        try
        {
            return await _context.Templates.AnyAsync(x => x.Id.Equals(id));
        }
        catch (Exception exception)
        {
            Log.Error("Failed to check if template exists => {@exception}", exception);
            throw new InvalidOperationException("Template not found", exception);
        }
    }

    public async Task<Template> EditTemplate()
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<Template> DeleteTemplate(Guid id)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}