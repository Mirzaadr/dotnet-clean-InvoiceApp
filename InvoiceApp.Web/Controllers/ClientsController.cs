using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models;
using MediatR;
using InvoiceApp.Application.Clients.Get;
using InvoiceApp.Application.Clients.GetById;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Clients.Create;
using InvoiceApp.Application.Clients.Delete;
using InvoiceApp.Application.Clients.Update;

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

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = "")
    {
        var query = new GetClientsQuery(page, pageSize, search);
        var result = await _mediator.Send(query);

        ViewBag.SearchTerm = search;
        return View(result);
    }

    public async Task<IActionResult> Details(Guid Id)
    {
        var query = new GetClientByIdQuery(Id);
        var result = await _mediator.Send(query);
        return View(result);
    }

    public IActionResult Create()
    {
        return View(new ClientDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientDto client)
    {
        var query = new CreateClientCommand(client);
        await _mediator.Send(query);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid Id)
    {
        var query = new GetClientByIdQuery(Id);
        var client = await _mediator.Send(query);

        if (client == null)
            return NotFound();
        return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ClientDto client)
    {
        var query = new UpdateClientCommand(client);
        await _mediator.Send(query);
        // Optionally, you can add a success message or log the action
        await Task.CompletedTask;
        return RedirectToAction("Index");
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _mediator.Send(new DeleteClientCommand(id));
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var query = new GetClientsQuery(1, 10, term);
        var clients = await _mediator.Send(query);
        return Json(clients.Items.Select(c => new { id = c.Id, text = c.Name }));
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetClientByIdQuery(id);
        var client = await _mediator.Send(query);

        return Json(new
        {
            name = client.Name,
            email = client.Email,
            phone = client.PhoneNumber,
            address = client.Address
        });
    }

}
