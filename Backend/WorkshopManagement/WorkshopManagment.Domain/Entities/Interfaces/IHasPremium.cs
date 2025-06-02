using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;

public interface IHasPremium
{
    public Percent Premium { get; set; }

    public Money PremiumPayment(Money basePayment) => basePayment * Premium.ToDecimal();
}