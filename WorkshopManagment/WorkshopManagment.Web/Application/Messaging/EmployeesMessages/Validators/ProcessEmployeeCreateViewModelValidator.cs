using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Validators;

public class ProcessEmployeeCreateViewModelValidator
    : AbstractValidator<ProcessEmployeeCreateViewModel>
{
    public ProcessEmployeeCreateViewModelValidator()
    {
        Include(new EmployeeCreateViewModelValidator());

        RuleFor(expression: x => x.Premium)
            .NotNull().WithMessage("Premium is required.")
            .GreaterThan(0).WithMessage("Premium must be greater than 0.");
    }
}