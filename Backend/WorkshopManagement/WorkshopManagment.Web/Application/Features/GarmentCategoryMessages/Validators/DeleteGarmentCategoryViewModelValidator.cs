using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.Validators;

public class DeleteGarmentCategoryViewModelValidator
    : AbstractValidator<DeleteGarmentCategoryViewModel>
{
    public DeleteGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Id)
        .NotEmpty().WithMessage("Category Id is required.");
}