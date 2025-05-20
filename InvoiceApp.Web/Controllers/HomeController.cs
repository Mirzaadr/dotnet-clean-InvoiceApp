using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models;
using MediatR;
using InvoiceApp.Application.Invoices.GetAll;
using InvoiceApp.Application.Invoices.Get;
using InvoiceApp.Application.DTOs;

namespace InvoiceApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISender _mediator;

    public HomeController(ILogger<HomeController> logger, ISender mediator)
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
