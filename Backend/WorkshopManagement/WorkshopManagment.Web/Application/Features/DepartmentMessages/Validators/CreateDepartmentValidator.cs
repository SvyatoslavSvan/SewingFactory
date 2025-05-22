using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Validators;

public class CreateDepartmentValidator : AbstractValidator<CreateRequest<CreateDepartmentViewModel, Department, ReadDepartmentViewModel>>
{
    public CreateDepartmentValidator() => RuleFor(expression: x => x.Model.Name).NotEmpty().WithMessage("Name cannot be Empty").MaximumLength(100);
}