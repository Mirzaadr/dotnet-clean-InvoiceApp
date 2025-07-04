using System.Security.Claims;
using InvoiceApp.Application.Users.Login;
using InvoiceApp.Web.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Web.Controllers;

public class AuthController : Controller
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var isAuth = await _mediator.Send(new LoginUserCommand(model.Username, model.Password));
        if (isAuth)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, model.Username)
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "username or password is incorrect");
            return View(model);
        }    
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login");
    }
    
    public IActionResult AccessDenied() => View();
}
