using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Queries;

public sealed record GetByIdProcessRequest(ClaimsPrincipal User, Guid Id)
    : GetByIdRequest<Process, ReadProcessViewModel>(User, Id);

public sealed class GetByIdProcessHandler(ApplicationDbContext dbContext, IMapper mapper)
    : GetByIdRequestHandler<Process, ReadProcessViewModel>(dbContext, mapper)
{
}