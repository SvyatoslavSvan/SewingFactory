using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Queries;

public sealed record UpdateDepartmentRequest(UpdateDepartmentViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<UpdateDepartmentViewModel, Department>(Model, User);

public sealed class UpdateDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : UpdateRequestHandler<UpdateDepartmentViewModel, Department>(unitOfWork, mapper);