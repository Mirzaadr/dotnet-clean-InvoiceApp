using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models;
using MediatR;
using InvoiceApp.Application.Home.GetSummary;

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
        var summary = await _mediator.Send(new GetSummaryQuery());

        return View(summary);
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
