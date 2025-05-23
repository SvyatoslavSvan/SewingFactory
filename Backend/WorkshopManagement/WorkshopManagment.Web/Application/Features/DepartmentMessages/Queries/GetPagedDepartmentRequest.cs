﻿using AutoMapper;
using Calabonga.UnitOfWork;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.Queries;

public sealed record GetPagedDepartmentRequest(ClaimsPrincipal User, int PageIndex, int PageSize)
    : GetPagedRequest<Department, ReadDepartmentViewModel>(User, PageIndex, PageSize);

public sealed class GetPagedDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : GetPagedRequestHandler<Department, ReadDepartmentViewModel>(unitOfWork, mapper);