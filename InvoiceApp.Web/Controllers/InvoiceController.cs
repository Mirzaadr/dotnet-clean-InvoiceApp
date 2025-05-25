using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models;
using MediatR;
using InvoiceApp.Application.Invoices.GetById;
using InvoiceApp.Application.Invoices.Get;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Invoices.Create;
using InvoiceApp.Application.Invoices.Delete;
using InvoiceApp.Web.Models.ViewModels;
using InvoiceApp.Application.Clients.Get;
using InvoiceApp.Application.Products.Get;
using InvoiceApp.Application.Invoices.Update;

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

  public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string search = "")
  {
    // var command = _mapper.Map<CreateMenuCommand>((request, hostId));
    var query = new GetInvoicesQuery(page, pageSize, search);
    var result = await _mediator.Send(query);
    ViewBag.SearchTerm = search;
    return View(result);
  }

  public async Task<IActionResult> Details(Guid id)
  {
    var query = new GetInvoiceByIdQuery(id);
    var result = await _mediator.Send(query);
    return View(result);
  }

  public async Task<IActionResult> Create()
  {
    var clients = await _mediator.Send(new GetClientsQuery(1, null, null));
    var products = await _mediator.Send(new GetProductsQuery(1, null, null));

    var newInvoice = new InvoiceDTO
    {
      Id = Guid.Empty,
      InvoiceNumber = "INV-100001",
      ClientId = Guid.Empty,
      IssueDate = DateTime.Now,
      DueDate = DateTime.Now.AddDays(30),
      TotalAmount = 0,
      Status = "Draft",
      Items = new List<InvoiceItemDto>()
    };

    var formData = new InvoiceFormViewModel
    {
      Invoice = newInvoice,
      Clients = clients.Items,
      Products = products.Items
    };
    return View(formData);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateInvoiceCommand command)
  {
    if (!ModelState.IsValid)
    {
      var clients = await _mediator.Send(new GetClientsQuery(1, null, null));
      var products = await _mediator.Send(new GetProductsQuery(1, null, null));

      var formData = new InvoiceFormViewModel
      {
        Invoice = invoice,
        Clients = clients.Items,
        Products = products.Items
      };
      return View(formData);
    }
    var command = new CreateInvoiceCommand(
      invoice.ClientId,
      invoice.IssueDate,
      invoice.DueDate,
      invoice.Items!
        .ConvertAll(
          item => new InvoiceItemCommand(
            item.ProductId,
            item.ProductName,
            item.Quantity,
            item.UnitPrice
          )
      )
    );
    await _mediator.Send(command);
    // return RedirectToAction(nameof(Details), new { id = newId });
    return RedirectToAction(nameof(Index));
  }

  public async Task<IActionResult> Edit(Guid id)
  {
    var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));
    if (invoice == null)
      return NotFound();

    var clients = await _mediator.Send(new GetClientsQuery(1, null, null));
    var products = await _mediator.Send(new GetProductsQuery(1, null, null));

    var formData = new InvoiceFormViewModel
    {
      Invoice = invoice,
      Clients = clients.Items,
      Products = products.Items
    };

    return View("Edit", formData);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(Guid id, [Bind(Prefix = "Invoice")]InvoiceDTO invoice)
  {
      if (id != invoice.Id)
          return BadRequest();

      if (!ModelState.IsValid)
      {
          var clients = await _mediator.Send(new GetClientsQuery(1, null, null));
          var products = await _mediator.Send(new GetProductsQuery(1, null, null));

          var formData = new InvoiceFormViewModel
          {
            Invoice = invoice,
            Clients = clients.Items,
            Products = products.Items
          };
          return View("Edit", formData);
      }

      var command = new UpdateInvoiceCommand(invoice);

      await _mediator.Send(command);
      return RedirectToAction(nameof(Index));
  }

  public async Task<IActionResult> Delete(Guid id)
  {
    var invoice = await _mediator.Send(new GetInvoiceByIdQuery(id));
    if (invoice == null) return NotFound();
    return View(invoice);
  }

  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(Guid id)
  {
    await _mediator.Send(new DeleteInvoiceCommand(id));
    return RedirectToAction(nameof(Index));
  }
}

