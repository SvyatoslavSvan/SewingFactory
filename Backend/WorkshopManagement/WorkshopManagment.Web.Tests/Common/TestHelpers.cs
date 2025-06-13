using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Mapping.Profiles;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Common;

public static class TestHelpers
{
    public static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(optionsAction: o =>
            o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddUnitOfWork<ApplicationDbContext>();
        
        services.AddAutoMapper(
            (sp, cfg) => cfg.ConstructServicesUsing(sp.GetService),
            typeof(EmployeeMappingProfile).Assembly  
        );

        return services.BuildServiceProvider();
    }

    public static Department SeedDepartment(IUnitOfWork<ApplicationDbContext> unitOfWork, string name = "Default Department")
    {
        var department = new Department(name);
        unitOfWork.GetRepository<Department>().Insert(department);
        unitOfWork.SaveChanges();

        return department;
    }

    public static GarmentCategory SeedGarmentCategory(IUnitOfWork<ApplicationDbContext> unitOfWork, string name = "Casual")
    {
        var category = new GarmentCategory(name);
        unitOfWork.GetRepository<GarmentCategory>().Insert(category);
        unitOfWork.SaveChanges();

        return category;
    }

    public static List<Process> SeedProcesses(IUnitOfWork<ApplicationDbContext> unitOfWork, Department department)
    {
        var repository = unitOfWork.GetRepository<Process>();

        var cutting = new Process("Cutting", department, new Money(5));
        var sewing  = new Process("Sewing",  department, new Money(7));

        repository.Insert(cutting);
        repository.Insert(sewing);
        unitOfWork.SaveChanges();

        return [ cutting, sewing ];
    }

    public static void SeedGarmentModels(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        GarmentCategory garmentCategory,
        List<Process> processes)
    {
        var repository = unitOfWork.GetRepository<GarmentModel>();

        repository.Insert(new GarmentModel(
            "Dress A",
            "Summer dress",
            processes,
            garmentCategory,new Money(1000)));

        repository.Insert(new GarmentModel(
            "Blouse B",
            "Office blouse",
            processes,
            garmentCategory, new Money(500)));

        repository.Insert(new GarmentModel(
            "Jacket C",
            "Warm jacket",
            processes,
            garmentCategory, new Money(150)));

        unitOfWork.SaveChanges();
    }
    
    public static (Department dept, GarmentModel model) SeedWorkshopDomain(
        IUnitOfWork<ApplicationDbContext> uow)
    {
        var dept      = SeedDepartment(uow);
        var category  = SeedGarmentCategory(uow);
        var processes = SeedProcesses(uow, dept);

        SeedGarmentModels(uow, category, processes);

        var model = uow.GetRepository<GarmentModel>()
            .GetFirstOrDefault(predicate: _ => true)!;

        return (dept, model);
    }

    public static WorkshopDocument SeedWorkshopDocument(
        IUnitOfWork<ApplicationDbContext> uow,
        string     name,
        int        countOfModels = 100,
        DateOnly?  date          = null)
    {
        var (dept, model) = SeedWorkshopDomain(uow);

        var doc = WorkshopDocument.CreateInstance(
            name,
            countOfModels,
            date ?? DateOnly.FromDateTime(DateTime.UtcNow),
            model,
            dept);

        uow.GetRepository<WorkshopDocument>().Insert(doc);
        uow.SaveChanges();

        return doc;
    }

    public static IReadOnlyList<Guid> SeedWorkshopDocuments(
        IUnitOfWork<ApplicationDbContext> uow,
        params string[] names)
    {
        var ids = new List<Guid>();
        foreach (var n in names)
        {
            ids.Add(SeedWorkshopDocument(uow, n).Id);
        }

        return ids;
    }
}