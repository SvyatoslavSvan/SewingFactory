using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Validatiors;

public sealed class DeleteProcessViewModelValidator
    : AbstractValidator<DeleteProcessViewModel>
{
    public DeleteProcessViewModelValidator() => RuleFor(expression: x => x.Id)
        .NotEmpty().WithMessage("Process Id is required.");
}