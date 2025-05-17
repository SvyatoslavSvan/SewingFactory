using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Queries;

public sealed record GetByIdDepartmentRequest(ClaimsPrincipal User, Guid Id)
    : GetByIdRequest<Department, ReadDepartmentViewModel>(User, Id);

public sealed class GetByIdDepartmentHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper)
    : GetByIdRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);