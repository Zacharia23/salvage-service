using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalvageCore.Extensions;
using SalvageCore.Interface;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceRepository _repository;

    public InvoiceController(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Route("[Controller]/FetchInvoices")]
    public async Task<IActionResult> FetchInvoicesAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _repository.FetchInvoices();

        return this.Respond("Invoices Fetched Successfully", result);
    }

    [HttpGet]
    [Route("[Controller]/FetchInvoice/{id:guid}")]
    public async Task<IActionResult> FetchInvoicesAction([FromRoute] Guid id)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _repository.FetchInvoiceDetails(id);

        return this.Respond("Invoice Details Fetched Successfully", result);
    }
}