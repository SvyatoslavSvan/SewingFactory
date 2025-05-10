using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Validators;

public class RateEmployeeCreateViewModelValidator
    : AbstractValidator<RateEmployeeCreateViewModel>
{
    public RateEmployeeCreateViewModelValidator()
    {
        Include(new ProcessEmployeeCreateViewModelValidator());

        RuleFor(expression: x => x.Rate)
            .NotNull().WithMessage("Rate is required.")
            .GreaterThan(0).WithMessage("Rate must be greater than 0.");
    }
}