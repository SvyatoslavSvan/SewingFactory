﻿using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Backend.IdentityServer.Web.Application.Features.Profile.ViewModels;

/// <summary>
///     Data transfer object for user registration
/// </summary>
public class RegisterViewModel
{
    /// <summary>
    ///     FirstName
    /// </summary>
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    /// <summary>
    ///     LastName
    /// </summary>
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    /// <summary>
    ///     Email
    /// </summary>
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    /// <summary>
    ///     Password
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    /// <summary>
    ///     Password confirmation
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;
}
