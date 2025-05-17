namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Base;

public abstract class WorkshopDocumentViewModel
{
    public required string Name { get; set; }
    public int CountOfModelsInvolved { get; set; }
    public DateOnly Date { get; set; }
}