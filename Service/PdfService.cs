using SalvageCore.Interface;
using SalvageCore.Models;

namespace SalvageCore.Service;

public class PdfService : IPdfService
{
    public async Task<string> GenerateCustomerInvoice(Invoice invoice)
    {
        try
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}