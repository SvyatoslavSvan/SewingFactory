using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Publisher;

public interface IGarmentCategoryPublisher
{
    public Task PublishCreatedAsync(GarmentCategory category);
    public Task PublishUpdatedAsync(GarmentCategory category);
}