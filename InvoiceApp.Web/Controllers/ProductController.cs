using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models;
using MediatR;
using InvoiceApp.Application.Invoices.GetAll;
using InvoiceApp.Application.Products.GetAll;

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

    public async Task<IActionResult> Index()
    {
        var query = new GetProductListQuery();
        var result = await _mediator.Send(query);
        return View(result);
    }

    public IActionResult Create()
    {
        return View();
    }
}
