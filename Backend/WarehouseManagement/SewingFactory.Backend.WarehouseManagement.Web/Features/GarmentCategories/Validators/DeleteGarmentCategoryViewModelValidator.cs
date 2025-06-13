using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Validators;

public class DeleteGarmentCategoryViewModelValidator
    : AbstractValidator<GarmentCategoryDeleteViewModel>
{
    public DeleteGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Id)
        .NotEmpty().WithMessage("Category Id is required.");
}
