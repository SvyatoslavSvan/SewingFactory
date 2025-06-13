using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Common.Domain.ValueObjects;
using System.Reflection;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Employees.Entites;

public sealed class RateAndPremiumPaymentTests
{
    [Fact]
    public void RatePayment_ShouldReturnFullRate_WhenFullHoursWorked_UsingReflection()
    {
        var employee = new RateBasedEmployee("E", "1", new Money(1000m), new Department("D"));
        var timesheet = Timesheet.CreateInstance(new List<RateBasedEmployee> { employee }, new DateOnly(2025, 5, 1));

        var tsProp = typeof(RateBasedEmployee)
            .GetProperty("Timesheets", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        Assert.NotNull(tsProp);
        var tsCollection = tsProp.GetValue(employee) as ICollection<Timesheet>;
        Assert.NotNull(tsCollection);
        tsCollection.Add(timesheet);
        var period = new DateRange(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31));
        var payment = ((IHasRate)employee).RatePayment(period);
        Assert.Equal(1000m, payment.Amount);
    }

    [Fact]
    public void PremiumPayment_ShouldReturnCorrectFraction()
    {
        IHasPremium hasPremium = new RateBasedEmployee("E", "2", new Money(0), new Department("D"), 10);
        var basePay = new Money(500m);
        var premiumPay = hasPremium.PremiumPayment(basePay);
        Assert.Equal(50m, premiumPay.Amount);
    }
}