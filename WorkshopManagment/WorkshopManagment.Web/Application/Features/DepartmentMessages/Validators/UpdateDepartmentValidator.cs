using FluentValidation;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Validators;

public class UpdateDepartmentValidator : AbstractValidator<UpdateRequest<UpdateDepartmentViewModel, Department>>
{
    public UpdateDepartmentValidator() => RuleFor(expression: x => x.Model.Name).NotEmpty().WithMessage("Name cannot be Empty").MaximumLength(100);
}