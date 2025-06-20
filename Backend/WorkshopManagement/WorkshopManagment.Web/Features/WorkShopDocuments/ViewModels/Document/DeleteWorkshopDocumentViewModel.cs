﻿using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;

public sealed class DeleteWorkshopDocumentViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}