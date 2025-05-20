using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models.ViewModels;

namespace InvoiceApp.Web.ViewComponents;

public class PaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(PaginationViewModel model)
    {
        return View(model);
    }
}