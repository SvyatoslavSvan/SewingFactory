using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

public sealed class ReadGarmentCategoryViewModel : GarmentCategoryViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}