using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Invoices.Response;
using SalvageCore.Interface;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Repository;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<InvoiceResponseList>> FetchInvoices()
    {
        try
        {
            var result = await _context.Invoices
                .Select(invoice => new InvoiceResponseList
                {
                    Id = invoice.Id,
                    InvoiceId = invoice.Id,
                    InvoiceNumber = invoice.Reference,
                    InvoiceDate = invoice.CreatedDate,
                    BidId = invoice.BidId,
                    BidReference = invoice.Bid.BidReference,
                    VehicleNumber = invoice.Bid.Offer.Vehicle.RegistrationNumber,
                    Make = invoice.Bid.Offer.Vehicle.Make.Name,
                    Model = invoice.Bid.Offer.Vehicle.Model.Name,
                    Amount = invoice.Amount,
                    CustomerPhone = invoice.Customer.Phone,
                    CustomerEmail = invoice.Customer.Email,
                    CustomerName = invoice.Customer.FirstName
                })
                .ToListAsync();

            return result;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch customer list => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<Invoice> FetchInvoiceDetails(Guid id)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch customer list => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }
}