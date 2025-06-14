﻿using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Mapping.Converters;

public sealed class DepartmentStubConverter(IUnitOfWork<ApplicationDbContext> unitOfWork) : ITypeConverter<Guid, Department>
{
    public Department Convert(Guid id, Department _, ResolutionContext __)
    {
        var db = unitOfWork.DbContext;
        var stub = new Department("STUB");
        db.Entry(stub).Property("Id").CurrentValue = id;
        db.Entry(stub).State = EntityState.Unchanged;

        return stub;
    }
}