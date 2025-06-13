using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Mapping.Converters;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ProcessStubConverter(ApplicationDbContext dbContext) : ITypeConverter<Guid, Process>
{
    public Process Convert(Guid source, Process destination, ResolutionContext context)
    {
        var stub = new Process("stub", new Department("stub"));
        dbContext.Entry(stub).Property("Id").CurrentValue = source;
        dbContext.Entry(stub).State = EntityState.Unchanged;

        return stub;
    }
}