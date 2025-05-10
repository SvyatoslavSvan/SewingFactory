using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Validatiors;

public sealed class UpdateProcessViewModelValidator
    : AbstractValidator<UpdateProcessViewModel>
{
    public UpdateProcessViewModelValidator()
    {
        RuleFor(expression: x => x.Id)
            .NotEmpty().WithMessage("Process Id is required.");

        RuleFor(expression: x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(expression: x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}