﻿using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;

public sealed class CreateWorkshopDocumentViewModel : WorkshopDocumentViewModel
{
    public Guid GarmentModelId { get; set; }
    public Guid DepartmentId { get; set; }
}