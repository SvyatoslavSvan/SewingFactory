using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Queries;

public sealed record GetAllProcessRequest(ClaimsPrincipal User)
    : GetAllRequest<Process, ReadProcessViewModel>(User);

public sealed class GetAllProcessHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetAllRequestHandler<Process, ReadProcessViewModel>(unitOfWork, mapper)
{
}