using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Queries;

public sealed record GetPagedDepartmentRequest(ClaimsPrincipal User, int PageIndex, int PageSize)
    : GetPagedRequest<Department, ReadDepartmentViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetPagedRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);