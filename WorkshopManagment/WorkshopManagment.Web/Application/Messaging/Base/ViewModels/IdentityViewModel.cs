using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels
{
    public abstract class IdentityViewModel
    {
        [Required(ErrorMessage = "Id is required")] public Guid Id { get; set; }
    }
}
