using AutoMapper;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using MediatR;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Providers;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Queries
{
    public record ManualCreateTimesheetRequest(ClaimsPrincipal User) : IRequest<OperationResult<TimesheetViewModel>>;

    public class ManualCreateTimesheetRequestHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IDateTimeProvider provider) : IRequestHandler<ManualCreateTimesheetRequest, OperationResult<TimesheetViewModel>>
    {
        public async Task<OperationResult<TimesheetViewModel>> Handle(
            ManualCreateTimesheetRequest request,
            CancellationToken cancellationToken)
        {
            var operationResult = OperationResult.CreateResult<TimesheetViewModel>();
            var timesheet = await unitOfWork.GetRepository<Timesheet>()
                .InsertAsync(Timesheet.CreateInstance(await unitOfWork.GetRepository<RateBasedEmployee>()
                            .GetAllAsync(trackingType: TrackingType.Tracking),
                        provider.CurrentMonthStart),
                    cancellationToken);
            await unitOfWork.SaveChangesAsync();
            if (!unitOfWork.Result.Ok)
            {
                operationResult.AddError(unitOfWork.Result.Exception);

                return operationResult;
            }
            operationResult.Result = mapper.Map<TimesheetViewModel>(
                timesheet.Entity,
                opts => opts.Items["Timesheet"] = timesheet.Entity
            );
            return operationResult;
        }
    }
}