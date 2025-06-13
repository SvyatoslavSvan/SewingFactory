namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels.Base;

public abstract class ProcessViewModel
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}