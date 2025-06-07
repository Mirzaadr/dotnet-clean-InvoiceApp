using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InvoiceApp.Application.Invoices.GetById;
using InvoiceApp.Application.Invoices.Get;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Invoices.Create;
using InvoiceApp.Application.Invoices.Delete;
using InvoiceApp.Application.Invoices.Update;
using InvoiceApp.Web.Services;
using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Web.Controllers;

public class InvoiceController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly ISender _mediator;
  private readonly IInvoiceFormViewModelFactory _formFactory;
  private readonly IInvoiceNumberGenerator _invoiceNumberGenerator;

  public InvoiceController(ILogger<HomeController> logger, ISender mediator, IInvoiceFormViewModelFactory formFactory, IInvoiceNumberGenerator invoiceNumberGenerator)
  {
    _logger = logger;
    _mediator = mediator;
    _formFactory = formFactory;
    _invoiceNumberGenerator = invoiceNumberGenerator;
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
    var newInvoiceNumber = await _invoiceNumberGenerator.GenerateAsync();
    var newInvoice = new InvoiceDTO
    {
      Id = Guid.Empty,
      InvoiceNumber = newInvoiceNumber,
      ClientId = Guid.Empty,
      IssueDate = DateTime.Now,
      DueDate = DateTime.Now.AddDays(30),
      TotalAmount = 0,
      Status = "Draft",
      Items = new List<InvoiceItemDto>()
    };

    var formData = await _formFactory.CreateAsync(newInvoice);
    return View(formData);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create([Bind(Prefix = "Invoice")] InvoiceDTO invoice)
  {
    ValidateItems(invoice);

    if (!ModelState.IsValid)
    {
      var formData = await _formFactory.CreateAsync(invoice);
      return View(formData);
    }
    var command = new CreateInvoiceCommand(
      invoice.ClientId,
      invoice.InvoiceNumber,
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

    if (invoice.Status != InvoiceStatus.Draft.ToString())
    {
      return RedirectToAction(nameof(Details), new { id = id });
    }

    var formData = await _formFactory.CreateAsync(invoice);

    return View("Edit", formData);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(Guid id, [Bind(Prefix = "Invoice")] InvoiceDTO invoice)
  {
      if (id != invoice.Id)
          return BadRequest();

      ValidateItems(invoice);

      if (!ModelState.IsValid)
      {
          var formData = await _formFactory.CreateAsync(invoice);
          return View("Edit", formData);
      }

      if (invoice.Status != InvoiceStatus.Draft.ToString())
      {
          ModelState.AddModelError("", "Cannot update sent invoice");
          var formData = await _formFactory.CreateAsync(invoice);
          return View("Edit", formData);
      }

      var command = new UpdateInvoiceCommand(invoice.Id, invoice.IssueDate, invoice.DueDate, invoice.Items);

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

  private void ValidateItems(InvoiceDTO invoice)
  {
      bool hasAnyValidItem = invoice.Items != null &&
                       invoice.Items.Any(item => item.ProductId != Guid.Empty);

      bool hasAnyInvalidItem = invoice.Items != null &&
                              invoice.Items.Any(item => item.ProductId == Guid.Empty);

      if (!hasAnyValidItem)
      {
        ModelState.AddModelError("Invoice.Items", "At least one item must be selected.");
      }
      else if (hasAnyInvalidItem)
      {
        ModelState.AddModelError("Invoice.Items", "One or more items are missing a selection.");
      }
  }
}

