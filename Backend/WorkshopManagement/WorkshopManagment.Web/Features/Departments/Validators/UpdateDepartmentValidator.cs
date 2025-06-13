using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Validators;

public class UpdateDepartmentValidator : AbstractValidator<UpdateRequest<UpdateDepartmentViewModel, Department>>
{
    public UpdateDepartmentValidator() => RuleFor(expression: x => x.Model.Name).NotEmpty().WithMessage("Name cannot be Empty").MaximumLength(100);
}