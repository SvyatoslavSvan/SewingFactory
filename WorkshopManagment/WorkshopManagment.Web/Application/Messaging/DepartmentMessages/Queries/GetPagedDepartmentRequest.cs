using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Queries;

public sealed record GetPagedDepartmentRequest(ClaimsPrincipal User, int PageIndex, int PageSize)
    : GetPagedRequest<Department, ReadDepartmentViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetPagedRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);