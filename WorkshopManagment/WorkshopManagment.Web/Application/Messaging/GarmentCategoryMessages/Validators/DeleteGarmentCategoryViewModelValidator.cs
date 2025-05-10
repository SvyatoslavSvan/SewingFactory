using FluentValidation;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.Validators;

public class DeleteGarmentCategoryViewModelValidator
    : AbstractValidator<DeleteGarmentCategoryViewModel>
{
    public DeleteGarmentCategoryViewModelValidator() => RuleFor(expression: x => x.Id)
        .NotEmpty().WithMessage("Category Id is required.");
}