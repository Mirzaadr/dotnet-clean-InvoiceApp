using Microsoft.AspNetCore.Mvc;
using MediatR;
using InvoiceApp.Application.Products.Get;
using InvoiceApp.Application.Products.GetById;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Products.Create;
using InvoiceApp.Application.Products.Delete;
using InvoiceApp.Application.Products.Update;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceApp.Web.Controllers;

[Authorize]
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
        return View(new ProductDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductCommand product)
    {
        await _mediator.Send(product);
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProductCommand product)
    {
        await _mediator.Send(product);
        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var query = new GetProductsQuery(1, 10, term);
        var products = await _mediator.Send(query);
        return Json(products.Items.Select(c => new { id = c.Id, text = c.Name }));
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);

        return Json(new {
            name = product.Name,
            unitPrice = product.Price
        });
    }

}
