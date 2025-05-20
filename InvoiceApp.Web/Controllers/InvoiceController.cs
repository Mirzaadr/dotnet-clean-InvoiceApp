using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models;
using MediatR;
using InvoiceApp.Application.Invoices.GetAll;
using InvoiceApp.Application.Invoices.Get;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Invoices.Create;
using InvoiceApp.Application.Invoices.Delete;

namespace InvoiceApp.Web.Controllers;

public class InvoiceController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly ISender _mediator;

  public InvoiceController(ILogger<HomeController> logger, ISender mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task<IActionResult> Index()
  {
    // var command = _mapper.Map<CreateMenuCommand>((request, hostId));
    var query = new GetInvoiceListQuery();
    var result = await _mediator.Send(query);
    return View(result);
  }

  public async Task<IActionResult> Details(Guid id)
  {
    var query = new GetInvoiceQuery(id);
    var result = await _mediator.Send(query);
    return View(result);
  }
    
    public IActionResult Create()
    {
        return View(new InvoiceFormDTO
        {
          Id = Guid.Empty,
          ClientId = Guid.Empty,
          InvoiceNumber = "",
          IssueDate = DateTime.Now,
          DueDate = DateTime.Now.AddDays(30),
          TotalAmount = 0,
          Status = "Draft",
          Items = new List<InvoiceItemDto>(),
          // ProductId = Guid.Empty,
          // ProductName = "",
          // Quantity = 0,
          // UnitPrice = 0,
          // LineTotal = 0
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceCommand command)
    {
        if (!ModelState.IsValid) return View(command);

        await _mediator.Send(command);
        // return RedirectToAction(nameof(Details), new { id = newId });
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var invoice = await _mediator.Send(new GetInvoiceQuery(id));
        if (invoice == null)
            return NotFound();

        var formDto = new InvoiceFormDTO
        {
            Id = invoice.Id,
            ClientId = invoice.ClientId,
            IssueDate = invoice.IssueDate,
            Items = invoice.Items
        };

        return View("Edit", formDto);
    }
    
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Edit(Guid id, InvoiceFormDTO model)
    // {
    //     if (id != model.Id)
    //         return BadRequest();

    //     if (!ModelState.IsValid)
    //         return View("Edit", model);

    //     var command = new UpdateInvoiceCommand
    //     {
    //         Id = model.Id,
    //         ClientId = model.ClientId,
    //         IssueDate = model.IssueDate,
    //         Items = model.Items
    //     };

    //     await _mediator.Send(command);
    //     return RedirectToAction(nameof(Index));
    // }

    public async Task<IActionResult> Delete(Guid id)
  {
    var invoice = await _mediator.Send(new GetInvoiceQuery(id));
    if (invoice == null) return NotFound();
    return View(invoice);
  }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _mediator.Send(new DeleteInvoiceCommand(id));
        return RedirectToAction(nameof(Index));
    }
}
