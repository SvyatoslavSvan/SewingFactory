using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.Queries
{
    public record UpdateWorkdayRequest(UpdateWorkdayViewModel Model,ClaimsPrincipal User) : UpdateRequest<UpdateWorkdayViewModel, WorkDay>(Model, User);

    public class UpdateWorkDayRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) : UpdateRequestHandler<UpdateWorkdayViewModel, WorkDay>(unitOfWork, mapper);
}
