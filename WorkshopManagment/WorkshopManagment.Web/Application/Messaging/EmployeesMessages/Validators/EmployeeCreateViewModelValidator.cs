using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Validators;

public class EmployeeCreateViewModelValidator
    : AbstractValidator<EmployeeCreateViewModel>
{
    public EmployeeCreateViewModelValidator()
    {
        RuleFor(expression: x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(expression: x => x.InternalId)
            .NotEmpty().WithMessage("Internal ID is required.");

        RuleFor(expression: x => x.Department)
            .IsInEnum().WithMessage("Invalid department.");
    }
}