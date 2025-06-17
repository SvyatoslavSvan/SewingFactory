using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Publisher;

public interface IGarmentModelPublisher
{
    public Task PublishCreatedAsync(GarmentModel garmentModel);
    public Task PublishUpdatedAsync(GarmentModel garmentModel);
}