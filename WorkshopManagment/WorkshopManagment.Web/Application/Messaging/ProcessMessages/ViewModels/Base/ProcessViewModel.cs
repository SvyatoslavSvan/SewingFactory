using SewingFactory.Backend.WorkshopManagement.Domain.Enums;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels.Base;

public abstract class ProcessViewModel
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}