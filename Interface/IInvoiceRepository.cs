using SalvageCore.DTOs.Invoices.Response;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface IInvoiceRepository
{
    public Task<ICollection<InvoiceResponseList>> FetchInvoices();
    public Task<Invoice> FetchInvoiceDetails(Guid id);
}