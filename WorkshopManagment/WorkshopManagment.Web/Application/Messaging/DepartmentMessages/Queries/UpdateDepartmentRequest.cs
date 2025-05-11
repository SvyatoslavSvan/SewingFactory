using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Queries;

public sealed record UpdateDepartmentRequest(UpdateDepartmentViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<UpdateDepartmentViewModel, Department>(Model, User);

public sealed class UpdateDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : UpdateRequestHandler<UpdateDepartmentViewModel, Department>(unitOfWork, mapper);