using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Queries;

public sealed record GetAllDepartmentsRequest(ClaimsPrincipal User)
    : GetAllRequest<Department, ReadDepartmentViewModel>(User);

public sealed class GetAllDepartmentsHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : GetAllRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);