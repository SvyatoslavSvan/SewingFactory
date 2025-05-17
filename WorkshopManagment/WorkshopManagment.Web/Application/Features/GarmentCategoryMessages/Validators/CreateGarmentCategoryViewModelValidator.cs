using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.Validators;

public sealed class CreateGarmentCategoryViewModelValidator
    : AbstractValidator<CreateRequest<CreateGarmentCategoryViewModel, GarmentCategory, ReadGarmentCategoryViewModel>>
{
    public CreateGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Model.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(100).WithMessage("Name must be at most 30 characters.");
}