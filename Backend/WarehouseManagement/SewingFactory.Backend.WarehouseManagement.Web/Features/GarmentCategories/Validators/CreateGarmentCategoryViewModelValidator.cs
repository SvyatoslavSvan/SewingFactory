using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Validators;

public sealed class CreateGarmentCategoryViewModelValidator
    : AbstractValidator<CreateRequest<GarmentCategoryCreateViewModel, GarmentCategory, GarmentCategoryReadViewModel>>
{
    public CreateGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Model.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
}
