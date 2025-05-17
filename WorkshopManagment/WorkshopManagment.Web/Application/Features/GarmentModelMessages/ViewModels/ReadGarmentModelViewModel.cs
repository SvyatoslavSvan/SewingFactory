using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels;

public class ReadGarmentModelViewModel : GarmentModelViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}