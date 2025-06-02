using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Validators;

public class UpdateWorkDayValidator : AbstractValidator<UpdateRequest<UpdateWorkdayViewModel, WorkDay>>
{
    public UpdateWorkDayValidator()
    {
        RuleFor(expression: x => x.Model.Id).NotEmpty();
        RuleFor(expression: x => x.Model.Hours).InclusiveBetween(0, 8);
    }
}