using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.Validators;

public sealed class DeleteProcessViewModelValidator
    : AbstractValidator<DeleteRequest<DeleteProcessViewModel, Process>>
{
    public DeleteProcessViewModelValidator() => RuleFor(expression: x => x.Model.Id)
        .NotEmpty().WithMessage("Process Id is required.");
}