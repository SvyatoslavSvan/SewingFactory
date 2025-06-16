using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Tests.Common;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Repeats;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Tasks;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.WorkshopDocuments.Handlers;

public sealed class WorkshopDocumentHandlerTests
{
    private readonly IMapper _mapper;
    private readonly IServiceProvider _sp;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public WorkshopDocumentHandlerTests()
    {
        _sp = TestHelpers.BuildServices();
        _unitOfWork = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        _mapper = _sp.GetRequiredService<IMapper>();
    }

    [Fact]
    public async Task CreateWorkshopDocumentHandler_Should_Create_Document()
    {
        // Arrange
        var (dept, model) = TestHelpers.SeedWorkshopDomain(_unitOfWork);

        var vm = new CreateWorkshopDocumentViewModel
        {
            Name = "May‑Batch‑Doc",
            CountOfModelsInvolved = 25,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            GarmentModelId = model.Id,
            DepartmentId = dept.Id
        };

        _unitOfWork.DbContext.ChangeTracker.Clear();
        var handler = new CreateWorkshopDocumentHandler(_unitOfWork, _mapper);
        var request = new CreateWorkshopDocumentRequest(vm, new ClaimsPrincipal());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert – the operation result
        Assert.True(result.Ok);
        Assert.NotNull(result.Result);
        Assert.Equal(vm.Name, result.Result!.Name);
        Assert.Equal(vm.CountOfModelsInvolved, result.Result!.CountOfModelsInvolved);
        Assert.Equal(vm.Date, result.Result!.Date);
        ;
        Assert.NotEqual(Guid.Empty, result.Result!.Id);
        Assert.Equal(model.Processes.Count, result.Result!.WorkshopTasks.Count);
        ;
    }

    [Fact]
    public async Task DeleteWorkshopDocumentHandler_Should_Delete_Document()
    {
        // Arrange
        var docId = TestHelpers.SeedWorkshopDocument(_unitOfWork, "Delete‑me").Id;
        _unitOfWork.DbContext.ChangeTracker.Clear();
        var vm = new DeleteWorkshopDocumentViewModel { Id = docId };

        var handler = new DeleteWorkshopDocumentHandler(_unitOfWork);
        var request = new DeleteWorkshopDocumentRequest(vm, new ClaimsPrincipal());
        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Ok);
        var ctx = _sp.GetRequiredService<ApplicationDbContext>();
        Assert.Empty(ctx.Set<WorkshopDocument>());
    }

    [Fact]
    public async Task GetAllWorkshopDocumentHandler_Should_Return_All()
    {
        // Arrange
        TestHelpers.SeedWorkshopDocuments(_unitOfWork, "Doc‑A", "Doc‑B", "Doc‑C");
        var handler = new GetAllWorkshopDocumentHandler(_unitOfWork, _mapper);
        var request = new GetAllWorkshopDocumentRequest(new ClaimsPrincipal());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Ok);
        Assert.Equal(3, result.Result!.Count());
    }

    [Fact]
    public async Task GetByIdWorkshopDocumentHandler_Should_Return_Document()
    {
        // Arrange
        var doc = TestHelpers.SeedWorkshopDocument(_unitOfWork, "Single‑Doc");
        var handler = new GetByIdWorkshopDocumentHandler(_unitOfWork, _mapper);
        var request = new GetByIdWorkshopDocumentRequest(new ClaimsPrincipal(), doc.Id);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Ok);
        Assert.Equal(doc.Id, result.Result!.Id);
    }

    [Theory]
    [InlineData(0, 2, 2)]
    [InlineData(1, 2, 1)] // page index starts from 0
    public async Task GetPagedWorkshopDocumentHandler_Should_Return_Paged(int pageIdx, int pageSize, int expected)
    {
        // Arrange
        TestHelpers.SeedWorkshopDocuments(_unitOfWork, "Pg‑1", "Pg‑2", "Pg‑3");
        var handler = new GetPagedWorkshopDocumentHandler(_unitOfWork, _mapper);
        var request = new GetPagedWorkshopDocumentRequest(new ClaimsPrincipal(), pageIdx, pageSize);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Ok);
        Assert.Equal(expected, result.Result!.Items.Count);
    }

    [Fact]
    public async Task GetForCreateWorkshopDocumentRequest_Should_Return_Lookups()
    {
        // Arrange
        TestHelpers.SeedWorkshopDomain(_unitOfWork);
        var handler = new GetForCreateWorkshopDocumentRequestHandler(_unitOfWork, _mapper);
        var request = new GetForCreateWorkshopDocumentRequest(new ClaimsPrincipal());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Ok);
        Assert.NotEmpty(result.Result!.GarmentModel);
        Assert.NotEmpty(result.Result!.Departments);
    }

    [Fact]
    public async Task GetForUpdateWorkshopDocumentRequest_Should_Return_Employees()
    {
        // Arrange – add one employee so that lookup is not empty
        await SeedProcessBasedEmployee();

        var handler = new GetForUpdateWorkshopDocumentRequestHandler(_unitOfWork, _mapper);
        var request = new GetForUpdateWorkshopDocumentRequest(new ClaimsPrincipal());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Ok);
        Assert.NotEmpty(result.Result!.Employees);
    }

    private async Task<ProcessBasedEmployee> SeedProcessBasedEmployee()
    {
        var dept = TestHelpers.SeedDepartment(_unitOfWork, "Lookup‑Dept");
        var processBasedEmployee = new ProcessBasedEmployee("Jon", "EMP‑01", dept);
        _unitOfWork.GetRepository<Employee>().Insert(processBasedEmployee);
        await _unitOfWork.SaveChangesAsync();

        return processBasedEmployee;
    }

    [Fact]
    public async Task UpdateWorkshopDocumentHandler_Should_Update_Document()
    {
        // Arrange
        var document = TestHelpers.SeedWorkshopDocument(_unitOfWork, "Initial‑Doc", 50);
        var processBasedEmployee = await SeedProcessBasedEmployee();
        var taskToAddRepeat = document.Tasks[0];
        const int validRepeatsCount = 50;
        var viewModel = new UpdateWorkshopDocumentViewModel
        {
            Id = document.Id,
            Name = "Updated‑Doc‑Name",
            CountOfModelsInvolved = 75,
            Date = document.Date,
            WorkshopTasks =
            [
                new UpdateWorkShopTaskViewModel
                {
                    EmployeeRepeats =
                    [
                        new UpdateEmployeeTaskRepeatViewModel { EmployeeId = processBasedEmployee.Id, Id = Guid.Empty, RepeatsCount = validRepeatsCount }
                    ],
                    Id = taskToAddRepeat.Id
                }
            ]
        };

        _unitOfWork.DbContext.ChangeTracker.Clear();
        var handler = new UpdateWorkshopDocumentHandler(_unitOfWork, _mapper);
        var request = new UpdateWorkshopDocumentRequest(viewModel, new ClaimsPrincipal());

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Ok);
        var updated = (await _unitOfWork.GetRepository<WorkshopDocument>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == document.Id,
                include: queryable => queryable.Include(navigationPropertyPath: workshopDocument => workshopDocument.Tasks)
                    .ThenInclude(navigationPropertyPath: task => task.EmployeeTaskRepeats).Include(navigationPropertyPath: workshopDocument => workshopDocument.Employees),
                trackingType: TrackingType.NoTracking))!;

        Assert.Equal(viewModel.Name, updated.Name);
        Assert.Equal(viewModel.CountOfModelsInvolved, updated.CountOfModelsInvolved);
        Assert.Equal(viewModel.Date, updated.Date);
        Assert.NotEmpty(viewModel.WorkshopTasks.First(predicate: x => x.Id == taskToAddRepeat.Id).EmployeeRepeats);
        ;
        Assert.Equal(validRepeatsCount, updated.Tasks.First(predicate: x => x.Id == taskToAddRepeat.Id).EmployeeTaskRepeats[0].Repeats);
        ;
        Assert.Single(updated.Employees);
    }
}