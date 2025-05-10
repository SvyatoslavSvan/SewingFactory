using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.Validators;

public class CreateGarmentCategoryViewModelValidator
    : AbstractValidator<CreateGarmentCategoryViewModel>
{
    public CreateGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(100).WithMessage("Name must be at most 100 characters.");
}