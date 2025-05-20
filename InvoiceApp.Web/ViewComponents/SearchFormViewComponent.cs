using Microsoft.AspNetCore.Mvc;
using InvoiceApp.Web.Models.ViewModels;

namespace InvoiceApp.Web.ViewComponents;

public class SearchFormViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(SearchFormViewModel model)
    {
        return View(model);
    }
}