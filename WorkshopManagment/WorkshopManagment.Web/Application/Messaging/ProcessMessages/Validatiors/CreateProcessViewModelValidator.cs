using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Validatiors;

public sealed class CreateProcessViewModelValidator
    : AbstractValidator<CreateProcessViewModel>
{
    public CreateProcessViewModelValidator()
    {
        RuleFor(expression: x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(expression: x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}