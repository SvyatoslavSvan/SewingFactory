﻿using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;

public sealed class CreateProcessViewModel : ProcessViewModel
{
    public Guid DepartmentId { get; set; }
}