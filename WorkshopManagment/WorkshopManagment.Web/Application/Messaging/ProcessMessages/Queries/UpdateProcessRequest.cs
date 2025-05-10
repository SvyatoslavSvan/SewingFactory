using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Queries;

public sealed record UpdateProcessRequest(UpdateProcessViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<UpdateProcessViewModel, Process>(Model, User);

public sealed class UpdateProcessHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : UpdateRequestHandler<UpdateProcessViewModel, Process>(unitOfWork, mapper)
{
}