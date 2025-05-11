// CreateDepartmentRequest.cs
using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums; 
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Queries;

public sealed record CreateDepartmentRequest(CreateDepartmentViewModel Model, ClaimsPrincipal User)
    : CreateRequest<CreateDepartmentViewModel, Department, ReadDepartmentViewModel>(Model, User);

public sealed class CreateDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : CreateRequestHandler<CreateDepartmentViewModel, Department, ReadDepartmentViewModel>(unitOfWork, mapper);