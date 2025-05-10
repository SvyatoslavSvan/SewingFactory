using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Queries;

public sealed record GetPagedProcessRequest(ClaimsPrincipal User, int PageIndex, int PageSize)
    : GetPagedRequest<Process, ReadProcessViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedProcessHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetPagedRequestHandler<Process, ReadProcessViewModel>(unitOfWork, mapper)
{
}