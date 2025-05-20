using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models;
using MediatR;
using InvoiceApp.Application.Clients.GetAll;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Clients.Get;

namespace InvoiceApp.Web.Controllers;

public class ClientsController : Controller
{
    private readonly ILogger<ClientsController> _logger;
    private readonly ISender _mediator;

    public ClientsController(ILogger<ClientsController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var query = new GetClientListQuery();
        var result = await _mediator.Send(query);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid Id)
    {
        var query = new GetClientQuery(Id);
        var result = await _mediator.Send(query);
        return View(result);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClientDto client)
    {
        // var query = new Create
        await Task.CompletedTask;
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(Guid Id)
    {
        var query = new GetClientQuery(Id);
        var client = await _mediator.Send(query);

        if (client == null)
            return NotFound();
        return View(client);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ClientDto client)
    {
        // var query = new Create
        await Task.CompletedTask;
        return RedirectToAction("Index");
    }
}
