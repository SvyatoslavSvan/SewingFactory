using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Queries;

public sealed record UpdateDepartmentRequest(UpdateDepartmentViewModel Model, ClaimsPrincipal User)
    : UpdateRequest<UpdateDepartmentViewModel, Department>(Model, User);

public sealed class UpdateDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : UpdateRequestHandler<UpdateDepartmentViewModel, Department>(unitOfWork, mapper);