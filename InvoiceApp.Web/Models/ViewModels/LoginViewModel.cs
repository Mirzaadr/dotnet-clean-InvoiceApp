using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.Web.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [DataType(DataType.Text)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}