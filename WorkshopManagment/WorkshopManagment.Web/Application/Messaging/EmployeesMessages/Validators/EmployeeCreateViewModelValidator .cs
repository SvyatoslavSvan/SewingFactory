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