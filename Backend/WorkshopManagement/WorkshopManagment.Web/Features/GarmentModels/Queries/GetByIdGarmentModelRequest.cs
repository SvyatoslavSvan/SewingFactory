using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;

public sealed record GetByIdGarmentModelRequest(ClaimsPrincipal User, Guid Id)
    : GetByIdRequest<GarmentModel, DetailsReadGarmentModelViewModel>(User, Id);

public sealed class GetByIdGarmentModelHandler(ApplicationDbContext dbContext, IMapper mapper)
    : GetByIdRequestHandler<GarmentModel, DetailsReadGarmentModelViewModel>(dbContext, mapper)
{
}