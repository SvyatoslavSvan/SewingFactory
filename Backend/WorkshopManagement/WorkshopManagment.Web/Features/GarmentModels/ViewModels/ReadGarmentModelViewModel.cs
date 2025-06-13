using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;

public class ReadGarmentModelViewModel : GarmentModelViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}