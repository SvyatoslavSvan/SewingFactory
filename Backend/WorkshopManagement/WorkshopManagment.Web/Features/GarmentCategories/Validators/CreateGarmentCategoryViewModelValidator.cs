using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Validators;

public sealed class CreateGarmentCategoryViewModelValidator
    : AbstractValidator<CreateRequest<CreateGarmentCategoryViewModel, GarmentCategory, ReadGarmentCategoryViewModel>>
{
    public CreateGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Model.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
}