using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Tests.Common;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Publisher;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Publisher;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.GarmentModels.Handlers;

public sealed class GarmentModelsHandlersTests
{
    private readonly IServiceProvider _serviceProvider = TestHelpers.BuildServices();

    private readonly ClaimsPrincipal _user = new(new ClaimsIdentity([
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Role, "Admin")
    ]));

    [Fact]
    public async Task CreateGarmentModelHandler_Should_Create_And_Return_GarmentModel()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var autoMapper = _serviceProvider.GetRequiredService<IMapper>();
        var publisherMock = SetupGarmentModelPublisherMock();
        var department = TestHelpers.SeedDepartment(unitOfWork);
        var garmentCategory = TestHelpers.SeedGarmentCategory(unitOfWork);
        var processes = TestHelpers.SeedProcesses(unitOfWork, department);

        var garmentCategoryId = garmentCategory.Id;
        var processIds = processes.Select(selector: process => process.Id).ToList();
        unitOfWork.DbContext.ChangeTracker.Clear();

        var createViewModel = new CreateGarmentModelViewModel
        {
            Name = "T-Shirt X",
            Description = "Cotton T-Shirt",
            GarmentCategoryId = garmentCategoryId,
            ProcessesIds = processIds,
            Price = 150m,
            Image = null
        };

        var request = new CreateGarmentModelRequest(createViewModel, _user);
        var handler = new CreateGarmentModelHandler(unitOfWork, autoMapper, publisherMock.Object);;

        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        var repository = unitOfWork.GetRepository<GarmentModel>();
        var savedModel = await repository.GetFirstOrDefaultAsync(
            predicate: model => model.Name == "T-Shirt X",
            include: query => query.Include(navigationPropertyPath: model => model.Category)
                .Include(navigationPropertyPath: model => model.Processes));
        publisherMock.Verify(x => x.PublishCreatedAsync(It.IsAny<GarmentModel>()), Times.Once, "Publisher was not called");
        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal("T-Shirt X", result.Result!.Name);
        Assert.NotNull(savedModel);
        Assert.Equal(garmentCategoryId, savedModel!.Category.Id);
        Assert.Equal(processIds.Count, savedModel.Processes.Count);
    }

    [Fact]
    public async Task GetByIdGarmentModelHandler_Should_Return_GarmentModel_When_Found()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var autoMapper = _serviceProvider.GetRequiredService<IMapper>();

        var department = TestHelpers.SeedDepartment(unitOfWork);
        var garmentCategory = TestHelpers.SeedGarmentCategory(unitOfWork);
        var processes = TestHelpers.SeedProcesses(unitOfWork, department);
        TestHelpers.SeedGarmentModels(unitOfWork, garmentCategory, processes);

        var repository = unitOfWork.GetRepository<GarmentModel>();
        var existingGarmentModel = await repository.GetFirstOrDefaultAsync(
            predicate: model => model.Name == "Dress A");

        Assert.NotNull(existingGarmentModel);

        var request = new GetByIdGarmentModelRequest(_user, existingGarmentModel!.Id);
        var handler = new GetByIdGarmentModelHandler(unitOfWork.DbContext, autoMapper);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(existingGarmentModel.Id, result.Result!.Id);
        Assert.Equal(existingGarmentModel.Name, result.Result.Name);
    }

    [Fact]
    public async Task GetAllGarmentModelHandler_Should_Return_All_GarmentModels()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var autoMapper = _serviceProvider.GetRequiredService<IMapper>();

        var department = TestHelpers.SeedDepartment(unitOfWork);
        var garmentCategory = TestHelpers.SeedGarmentCategory(unitOfWork);
        var processes = TestHelpers.SeedProcesses(unitOfWork, department);
        TestHelpers.SeedGarmentModels(unitOfWork, garmentCategory, processes);

        var request = new GetAllGarmentModelRequest(_user);
        var handler = new GetAllGarmentModelHandler(unitOfWork, autoMapper);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Result!.Count());
    }

    [Fact]
    public async Task GetPagedGarmentModelHandler_Should_Return_Paged_GarmentModels()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var autoMapper = _serviceProvider.GetRequiredService<IMapper>();

        var department = TestHelpers.SeedDepartment(unitOfWork);
        var garmentCategory = TestHelpers.SeedGarmentCategory(unitOfWork);
        var processes = TestHelpers.SeedProcesses(unitOfWork, department);
        TestHelpers.SeedGarmentModels(unitOfWork, garmentCategory, processes);

        var request = new GetPagedGarmentModelRequest(_user, 0, 2);
        var handler = new GetPagedGarmentModelHandler(unitOfWork, autoMapper);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Result!.TotalCount);
        Assert.Equal(2, result.Result.Items.Count);
    }

    [Fact]
    public async Task UpdateGarmentModelHandler_Should_Update_GarmentModel()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var autoMapper = _serviceProvider.GetRequiredService<IMapper>(); 
        var publisherMock = SetupGarmentModelPublisherMock();
        var department = TestHelpers.SeedDepartment(unitOfWork);
        var initialCategory = TestHelpers.SeedGarmentCategory(unitOfWork, "Initial Category");
        var initialProcesses = TestHelpers.SeedProcesses(unitOfWork, department);
        TestHelpers.SeedGarmentModels(unitOfWork, initialCategory, initialProcesses);

        var repository = unitOfWork.GetRepository<GarmentModel>();
        var trackedGarmentModel = await repository.GetFirstOrDefaultAsync(
            predicate: model => model.Name == "Dress A",
            trackingType: TrackingType.NoTracking);

        Assert.NotNull(trackedGarmentModel);

        var newCategory = TestHelpers.SeedGarmentCategory(unitOfWork, "Updated Category");
        var newProcesses = TestHelpers.SeedProcesses(unitOfWork, department);

        unitOfWork.DbContext.ChangeTracker.Clear();

        var updateViewModel = new UpdateGarmentModelViewModel
        {
            Id = trackedGarmentModel!.Id,
            Name = "Dress A Updated",
            Description = "Updated description",
            GarmentCategoryId = newCategory.Id,
            ProcessesIds = newProcesses.Select(selector: p => p.Id).ToList(),
            Image = null
        };

        var request = new UpdateGarmentModelRequest(updateViewModel, _user);
        var handler = new UpdateGarmentModelHandler(unitOfWork, autoMapper, publisherMock.Object);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var updatedGarmentModel = await repository.GetFirstOrDefaultAsync(
            predicate: model => model.Id == trackedGarmentModel.Id,
            include: query => query.Include(navigationPropertyPath: m => m.Category).Include(navigationPropertyPath: m => m.Processes),
            trackingType: TrackingType.NoTracking);
        publisherMock.Verify(x => x.PublishUpdatedAsync(It.IsAny<GarmentModel>()), Times.Once, "Publisher was not called");
        Assert.NotNull(updatedGarmentModel);
        Assert.Equal("Dress A Updated", updatedGarmentModel!.Name);
        Assert.Equal(newCategory.Id, updatedGarmentModel.Category.Id);
        Assert.Equal(newProcesses.Count, updatedGarmentModel.Processes.Count);
    }

    [Fact]
    public async Task DeleteGarmentModelHandler_Should_Delete_GarmentModel()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var department = TestHelpers.SeedDepartment(unitOfWork);
        var garmentCategory = TestHelpers.SeedGarmentCategory(unitOfWork);
        var processes = TestHelpers.SeedProcesses(unitOfWork, department);
        TestHelpers.SeedGarmentModels(unitOfWork, garmentCategory, processes);

        unitOfWork.DbContext.ChangeTracker.Clear();

        var repository = unitOfWork.GetRepository<GarmentModel>();
        var garmentToDelete = await repository.GetFirstOrDefaultAsync(
            predicate: model => model.Name == "Blouse B",
            trackingType: TrackingType.NoTracking);

        Assert.NotNull(garmentToDelete);

        var deleteViewModel = new DeleteGarmentModelViewModel { Id = garmentToDelete!.Id };
        var request = new DeleteGarmentModelRequest(deleteViewModel, _user);
        var handler = new DeleteGarmentModelHandler(unitOfWork);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var deletedGarmentModel = await repository.GetFirstOrDefaultAsync(
            predicate: model => model.Id == garmentToDelete.Id,
            trackingType: TrackingType.NoTracking);

        Assert.Null(deletedGarmentModel);
        Assert.Equal(2, repository.GetAll(TrackingType.NoTracking).Count());
    }

    [Fact]
    public async Task GetForCreateGarmentModelHandler_Should_Return_Categories_And_Processes()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var autoMapper = _serviceProvider.GetRequiredService<IMapper>();

        var department = TestHelpers.SeedDepartment(unitOfWork);
        var garmentCategory = TestHelpers.SeedGarmentCategory(unitOfWork);
        _ = TestHelpers.SeedProcesses(unitOfWork, department);

        var request = new GetForCreateGarmentModelRequest(_user);
        var handler = new GetForCreateGarmentModelRequestHandler(unitOfWork, autoMapper);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Result);
        Assert.NotEmpty(result.Result!.GarmentCategories);
        Assert.NotEmpty(result.Result.Processes);
    }
    
    private static Mock<IGarmentModelPublisher> SetupGarmentModelPublisherMock()
    {
        var garmentPublisherMock = new Mock<IGarmentModelPublisher>();
        garmentPublisherMock
            .Setup(x => x.PublishCreatedAsync(It.IsAny<GarmentModel>()))
            .Returns(Task.CompletedTask);
        return garmentPublisherMock;
    }
}
