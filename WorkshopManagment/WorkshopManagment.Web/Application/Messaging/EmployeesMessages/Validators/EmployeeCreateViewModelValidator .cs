using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Validators;

public class EmployeeCreateViewModelValidator
    : AbstractValidator<EmployeeCreateViewModel>
{
    public EmployeeCreateViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.InternalId)
            .NotEmpty().WithMessage("Internal ID is required.");

        RuleFor(x => x.Department)
            .IsInEnum().WithMessage("Invalid department.");

        RuleFor(x => x)
            .Must(x => new[]
                {
                    x.Premium.HasValue,
                    x.Rate.HasValue,
                    x.SalaryPercentage.HasValue
                }
                .Count(flag => flag) == 1
            )
            .WithMessage("Specify exactly one of: Premium, Rate, or SalaryPercentage.");

        When(x => x.Premium.HasValue, () =>
            RuleFor(x => x.Premium!.Value)
                .GreaterThan(0).WithMessage("Premium must be greater than 0."));

        When(x => x.Rate.HasValue, () =>
            RuleFor(x => x.Rate!.Value)
                .GreaterThan(0).WithMessage("Rate must be greater than 0."));

        When(x => x.SalaryPercentage.HasValue, () =>
            RuleFor(x => x.SalaryPercentage!.Value)
                .InclusiveBetween(0, 100)
                .WithMessage("SalaryPercentage must be between 0 and 100."));
    }
}