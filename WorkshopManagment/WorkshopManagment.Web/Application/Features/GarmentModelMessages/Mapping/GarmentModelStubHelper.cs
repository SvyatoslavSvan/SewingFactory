using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.Mapping;

public static class GarmentModelStubHelper
{
    public static GarmentCategory GetGarmentCategoryStub(PostGarmentModelViewModel source, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        var categoryStub = new GarmentCategory("stub", []);
        unitOfWork.DbContext.Entry(categoryStub).Property("Id").CurrentValue = source.GarmentCategoryId;
        unitOfWork.DbContext.Entry(categoryStub).State = EntityState.Unchanged;

        return categoryStub;
    }

    public static List<Process> GetProcessStubs(PostGarmentModelViewModel source, IUnitOfWork<ApplicationDbContext> unitOfWork) => source.ProcessesIds
        .Select(selector: id =>
        {
            var stub = new Process("stub", new Department("stub"));
            var entry = unitOfWork.DbContext.Entry(stub);
            entry.Property("Id").CurrentValue = id;
            entry.State = EntityState.Unchanged;

            return stub;
        })
        .ToList();
}