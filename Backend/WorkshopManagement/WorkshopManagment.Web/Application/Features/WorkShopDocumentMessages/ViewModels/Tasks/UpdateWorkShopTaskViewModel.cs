﻿using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Repeats;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Tasks;

public class UpdateWorkShopTaskViewModel : IIdentityViewModel
{
    public List<UpdateEmployeeTaskRepeatViewModel> EmployeeRepeats { get; set; } = null!;
    public Guid Id { get; set; }
}