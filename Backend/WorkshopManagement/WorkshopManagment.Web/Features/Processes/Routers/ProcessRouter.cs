using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Routers;

public class ProcessRouter : CommandRouter<Process, ReadProcessViewModel, CreateProcessViewModel, UpdateProcessViewModel, DeleteProcessViewModel, ReadProcessViewModel>
{
    protected override string PolicyName => AppData.DesignerAccess;
}