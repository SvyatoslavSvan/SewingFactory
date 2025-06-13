using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Validators;

public sealed class DeleteProcessViewModelValidator
    : AbstractValidator<DeleteRequest<DeleteProcessViewModel, Process>>
{
    public DeleteProcessViewModelValidator() => RuleFor(expression: x => x.Model.Id)
        .NotEmpty().WithMessage("Process Id is required.");
}