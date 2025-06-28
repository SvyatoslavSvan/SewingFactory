using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Backend.IdentityServer.Web.Pages.Connect;

public sealed class LoginViewModel
{
    [Required] [Display(Name = "User name")] public string UserName { get; set; } = null!;

    [Required] [Display(Name = "Password")] public string Password { get; set; } = null!;

    [Required] public string? ReturnUrl { get; set; } = null!;
}
