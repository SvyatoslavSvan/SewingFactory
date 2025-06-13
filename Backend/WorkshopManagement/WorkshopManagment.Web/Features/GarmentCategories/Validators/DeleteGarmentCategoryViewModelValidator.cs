using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Validators;

public class DeleteGarmentCategoryViewModelValidator
    : AbstractValidator<DeleteGarmentCategoryViewModel>
{
    public DeleteGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Id)
        .NotEmpty().WithMessage("Category Id is required.");
}