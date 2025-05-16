using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Queries;

public sealed record GetAllDepartmentsRequest(ClaimsPrincipal User)
    : GetAllRequest<Department, ReadDepartmentViewModel>(User);

public sealed class GetAllDepartmentsHandler(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    IMapper mapper) : GetAllRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);