using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.Microservices.Core;
using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints
{
    public class ProcessBasedEmployeeEndpoints : AppDefinition
    {

        public override void ConfigureApplication(WebApplication app)
        {
            app.MapPost("/api/ProcessBasedEmployee/create", CreateProcessBasedEmployee);
            app.MapPost("/api/ProcessBasedEmployee/delete", DeleteProcessBasedEmployee);
            app.MapPut("/api/ProcessBasedEmployee/update", UpdateProcessBasedEmployee);
            app.MapGet("/api/Employee/getAll", GetAllEmployees);
            app.MapGet("/api/Employee/getPaged/{pageIndex:int}/{pageSize:int}", GetPagedEmployees);
            app.MapGet("/api/Employee/getById/{Id:guid}", GetEmployeeById);
        }

        private const string _featureGroupName = "Employee";

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)] //TODO: Uncomment when authentication is ready
        private async Task<OperationResult<IdentityProcessBasedEmployeeViewModel>> CreateProcessBasedEmployee(
            [FromBody] ProcessBasedEmployeeViewModel model,
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new CreateProcessBasedEmployeeRequest(model,
                    context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<DeleteProcessBasedEmployeeViewModel>> DeleteProcessBasedEmployee(
            [FromBody] DeleteProcessBasedEmployeeViewModel model,
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new DeleteProcessBasedEmployeeRequest(model,
                    context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<IdentityProcessBasedEmployeeViewModel>> UpdateProcessBasedEmployee(
            [FromBody] IdentityProcessBasedEmployeeViewModel model,
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new UpdateProcessBasedEmployeeRequest(model,
                    context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<IEnumerable<Employee>>> GetAllEmployees(
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new GetAllEmployeesRequest(
                    context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<IPagedList<Employee>>> GetPagedEmployees(
            [FromServices] IMediator mediator,
            HttpContext context, int pageIndex, int pageSize)
            => await mediator.Send(new GetPagedEmployeesRequest(
                    context.User, pageIndex, pageSize),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<Employee>> GetEmployeeById(
            [FromServices] IMediator mediator,
            HttpContext context, Guid id)
            => await mediator.Send(new GetByIdEmployeeRequest(
                    context.User, id),
                context.RequestAborted);

    }
}
