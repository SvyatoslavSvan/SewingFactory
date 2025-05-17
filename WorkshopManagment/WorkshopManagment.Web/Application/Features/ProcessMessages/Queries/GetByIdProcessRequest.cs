using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.Queries;

public sealed record GetByIdProcessRequest(ClaimsPrincipal User, Guid Id)
    : GetByIdRequest<Process, ReadProcessViewModel>(User, Id);

public sealed class GetByIdProcessHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper)
    : GetByIdRequestHandler<Process, ReadProcessViewModel>(unitOfWork, mapper)
{
}