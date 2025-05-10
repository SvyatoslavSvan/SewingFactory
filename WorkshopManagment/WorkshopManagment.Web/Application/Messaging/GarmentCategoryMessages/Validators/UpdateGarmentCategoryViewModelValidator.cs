using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.Validators;

public class UpdateGarmentCategoryViewModelValidator
    : AbstractValidator<UpdateGarmentCategoryViewModel>
{
    public UpdateGarmentCategoryViewModelValidator()
    {
        RuleFor(expression: x => x.Id)
            .NotEmpty().WithMessage("Category Id is required.");

        RuleFor(expression: x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters.");
    }
}