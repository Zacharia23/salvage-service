using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface IPdfService
{
    Task<string> GenerateCustomerInvoice(Invoice invoice);
}