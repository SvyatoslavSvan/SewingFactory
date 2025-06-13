using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Routers;

public sealed class EmployeeRouter : CommandRouter<Employee, EmployeeReadViewModel, EmployeeCreateViewModel, EmployeeUpdateViewModel, EmployeeDeleteViewModel, EmployeeReadViewModel>
{
    public override void ConfigureApplication(WebApplication app)
    {
        base.ConfigureApplication(app);
        app.MapGet(Prefix + "/GetSalaryReport", GetSalaryReport).WithTags(_featureGroupName);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
    public async Task<IResult> GetSalaryReport(
        [FromServices] IMediator mediator, [FromQuery] DateOnly from, [FromQuery] DateOnly to,
        HttpContext context)
        => await mediator.Send(new GetSalaryReportRequest(context.User, new DateRange(from, to)),
            context.RequestAborted);
}