using Calabonga.AspNetCore.AppDefinitions;
using Calabonga.Microservices.Core;
using Calabonga.OperationResults;
using Calabonga.PagedListCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints
{
    public class ProcessBasedEmployeeEndpoints : AppDefinition
    {

        public override void ConfigureApplication(WebApplication app)
        {
            app.MapPost("/api/Employee/create", CreateProcessBasedEmployee);
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
        private async Task<OperationResult<EmployeeReadViewModel>> CreateProcessBasedEmployee(
            [FromBody] EmployeeCreateViewModel model,
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new CreateEmployeeRequest(model,
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
            => await mediator.Send(new DeleteRequest<DeleteProcessBasedEmployeeViewModel, ProcessBasedEmployee>(model,
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
            => await mediator.Send(new UpdateRequest<IdentityProcessBasedEmployeeViewModel,ProcessBasedEmployee>(model,
                    context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<IEnumerable<EmployeeReadViewModel>>> GetAllEmployees(
            [FromServices] IMediator mediator,
            HttpContext context)
            => await mediator.Send(new GetAllRequest<Employee,EmployeeReadViewModel>(
                    context.User),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<IPagedList<EmployeeReadViewModel>>> GetPagedEmployees(
            [FromServices] IMediator mediator,
            HttpContext context, int pageIndex, int pageSize)
            => await mediator.Send(new GetPagedRequest<Employee,EmployeeReadViewModel>(
                    context.User, pageIndex, pageSize),
                context.RequestAborted);

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [FeatureGroupName(_featureGroupName)]
        //[Authorize(AuthenticationSchemes = AuthData.AuthSchemes)]
        private async Task<OperationResult<EmployeeReadViewModel>> GetEmployeeById(
            [FromServices] IMediator mediator,
            HttpContext context, Guid id)
            => await mediator.Send(new GetByIdRequest<Employee,EmployeeReadViewModel>(
                    context.User, id),
                context.RequestAborted);

    }
}
