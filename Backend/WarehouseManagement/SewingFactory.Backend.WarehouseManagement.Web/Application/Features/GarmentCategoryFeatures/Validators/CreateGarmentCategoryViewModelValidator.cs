using FluentValidation;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.Validators;

public sealed class CreateGarmentCategoryViewModelValidator
    : AbstractValidator<CreateRequest<GarmentCategoryCreateViewModel, GarmentCategory, GarmentCategoryReadViewModel>>
{
    public CreateGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Model.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
}
