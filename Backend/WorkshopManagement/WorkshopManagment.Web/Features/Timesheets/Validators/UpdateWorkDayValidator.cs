using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Validators;

public class UpdateWorkDayValidator : AbstractValidator<UpdateRequest<UpdateWorkdayViewModel, WorkDay>>
{
    public UpdateWorkDayValidator()
    {
        RuleFor(expression: x => x.Model.Id).NotEmpty();
        RuleFor(expression: x => x.Model.Hours).InclusiveBetween(0, 8);
    }
}