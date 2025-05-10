using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Validators;

public class TechnologistCreateViewModelValidator
    : AbstractValidator<TechnologistCreateViewModel>
{
    public TechnologistCreateViewModelValidator()
    {
        Include(new EmployeeCreateViewModelValidator());

        RuleFor(expression: x => x.SalaryPercentage)
            .InclusiveBetween(0, 100)
            .WithMessage("SalaryPercentage must be between 0 and 100.");
    }
}