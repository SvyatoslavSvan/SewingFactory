using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Queries;

public sealed record GetAllDepartmentsRequest(ClaimsPrincipal User)
    : GetAllRequest<Department, ReadDepartmentViewModel>(User);

public sealed class GetAllDepartmentsHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : GetAllRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);