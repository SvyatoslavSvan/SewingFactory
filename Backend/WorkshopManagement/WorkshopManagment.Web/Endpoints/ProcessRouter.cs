using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints;

public class ProcessRouter : CommandRouter<Process, ReadProcessViewModel, CreateProcessViewModel, UpdateProcessViewModel, DeleteProcessViewModel, ReadProcessViewModel>
{
}