using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Validators;

public class DeleteGarmentCategoryViewModelValidator
    : AbstractValidator<GarmentCategoryDeleteViewModel>
{
    public DeleteGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Id)
        .NotEmpty().WithMessage("Category Id is required.");
}
