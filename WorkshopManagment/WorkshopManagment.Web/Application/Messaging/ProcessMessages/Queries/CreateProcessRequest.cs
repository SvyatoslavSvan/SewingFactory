using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.Queries;

public sealed record CreateProcessRequest(CreateProcessViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateProcessViewModel, Process, ReadProcessViewModel>(Model, User);

public sealed class CreateProcessHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : CreateRequestHandler<CreateProcessViewModel, Process, ReadProcessViewModel>(unitOfWork, mapper)
{
}