using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.Mapping.Profiles;

public sealed class DepartmentStubConverter
    : ITypeConverter<Guid, Department>
{
    private readonly IUnitOfWork<ApplicationDbContext> _uow;

    public DepartmentStubConverter(IUnitOfWork<ApplicationDbContext> uow)
        => _uow = uow;

    public Department Convert(Guid id, Department _, ResolutionContext __)
    {
        var db = _uow.DbContext;
        var stub = new Department("STUB");
        db.Entry(stub).Property("Id").CurrentValue = id;
        db.Entry(stub).State = EntityState.Unchanged;
        return stub;
    }
}