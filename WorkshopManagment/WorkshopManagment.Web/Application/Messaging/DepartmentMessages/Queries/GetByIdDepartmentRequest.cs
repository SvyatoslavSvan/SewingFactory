using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Queries;

public sealed record GetByIdDepartmentRequest(ClaimsPrincipal User, Guid Id)
    : GetByIdRequest<Department, ReadDepartmentViewModel>(User, Id);

public sealed class GetByIdDepartmentHandler(IUnitOfWork<ApplicationDbContext> unitOfWork, IMapper mapper)
    : GetByIdRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);