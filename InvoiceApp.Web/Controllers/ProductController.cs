using Microsoft.AspNetCore.Mvc;
using MediatR;
using InvoiceApp.Application.Products.Get;
using InvoiceApp.Application.Products.GetById;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Products.Create;
using InvoiceApp.Application.Products.Delete;

namespace InvoiceApp.Web.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ISender _mediator;

    public ProductsController(ILogger<ProductsController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = "")
    {
        var query = new GetProductsQuery(page, pageSize, search);
        var result = await _mediator.Send(query);

        ViewBag.SearchTerm = search;
        return View(result);
    }

    public async Task<IActionResult> Details(Guid Id)
    {
        var query = new GetProductByIdQuery(Id);
        var result = await _mediator.Send(query);
        return View(result);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto product)
    {
        var query = new CreateProductCommand(product);
        await _mediator.Send(query);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid Id)
    {
        var query = new GetProductByIdQuery(Id);
        var product = await _mediator.Send(query);

        if (product == null)
            return NotFound();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductDto product)
    {
        // var query = new Create
        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return RedirectToAction(nameof(Index));
    }
}
