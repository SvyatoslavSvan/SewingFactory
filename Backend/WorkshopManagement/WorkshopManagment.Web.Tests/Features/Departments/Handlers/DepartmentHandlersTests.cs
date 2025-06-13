using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Tests.Common;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Departments.Handlers;

public sealed class DepartmentHandlersTests
{
    private readonly IServiceProvider _serviceProvider = TestHelpers.BuildServices();
    private readonly ClaimsPrincipal _claimsPrincipal = new(new ClaimsIdentity([
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Role, "Admin")
    ]));

    // Mock user for authorization

    private void SeedTestData(IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        var departments = new List<Domain.Entities.Employees.Department>
        {
            new("Cutting Department"),
            new("Sewing Department"),
            new("Quality Control")
        };

        var repository = unitOfWork.GetRepository<Domain.Entities.Employees.Department>();
        foreach (var department in departments)
        {
            repository.Insert(department);
        }
        
        unitOfWork.SaveChanges();
    }

    [Fact]
    public async Task CreateDepartmentHandler_Should_Create_And_Return_Department()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _serviceProvider.GetRequiredService<IMapper>();
        
        var handler = new CreateDepartmentHandler(unitOfWork, mapper);
        var model = new CreateDepartmentViewModel { Name = "Test Department" };
        var request = new CreateDepartmentRequest(model, _claimsPrincipal);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Department", result.Result!.Name);
        
        // Verify the department was saved to the database
        var repository = unitOfWork.GetRepository<Domain.Entities.Employees.Department>();
        var savedDepartment = await repository.GetFirstOrDefaultAsync(predicate: d => d.Name == "Test Department");
        Assert.NotNull(savedDepartment);
    }
    
    [Fact]
    public async Task GetByIdDepartmentHandler_Should_Return_Department_When_Found()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _serviceProvider.GetRequiredService<IMapper>();
        
        SeedTestData(unitOfWork);
        
        var repository = unitOfWork.GetRepository<Domain.Entities.Employees.Department>();
        var existingDepartment = await repository.GetFirstOrDefaultAsync(predicate: d => d.Name == "Cutting Department");
        Assert.NotNull(existingDepartment);
        
        var handler = new GetByIdDepartmentHandler(unitOfWork, mapper);
        var request = new GetByIdDepartmentRequest(_claimsPrincipal, existingDepartment.Id);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Cutting Department", result.Result!.Name);
        Assert.Equal(existingDepartment.Id, result.Result!.Id);
    }
    
    [Fact]
    public async Task GetAllDepartmentsHandler_Should_Return_All_Departments()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _serviceProvider.GetRequiredService<IMapper>();
        
        SeedTestData(unitOfWork);
        
        var handler = new GetAllDepartmentsHandler(unitOfWork, mapper);
        var request = new GetAllDepartmentsRequest(_claimsPrincipal);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Result!.Count());
        Assert.Contains(result.Result!, d => d.Name == "Cutting Department");
        Assert.Contains(result.Result!, d => d.Name == "Sewing Department");
        Assert.Contains(result.Result!, d => d.Name == "Quality Control");
    }
    
    [Fact]
    public async Task GetPagedDepartmentHandler_Should_Return_Paged_Departments()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _serviceProvider.GetRequiredService<IMapper>();
        
        SeedTestData(unitOfWork);
        
        var handler = new GetPagedDepartmentHandler(unitOfWork, mapper);
        var request = new GetPagedDepartmentRequest(_claimsPrincipal, 0, 2);
        
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Result!.TotalCount); // Total of 3 departments
        Assert.Equal(2, result.Result.Items.Count); // But only 2 per page
    }
    
    [Fact]
    public async Task UpdateDepartmentHandler_Should_Update_Department()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _serviceProvider.GetRequiredService<IMapper>();
        
        SeedTestData(unitOfWork);
        
        var repository = unitOfWork.GetRepository<Domain.Entities.Employees.Department>();
        var existingDepartment = await repository.GetFirstOrDefaultAsync(
        predicate: d => d.Name == "Cutting Department", 
        trackingType: TrackingType.NoTracking); // Get without tracking
        Assert.NotNull(existingDepartment);
        
        var updateViewModel = new UpdateDepartmentViewModel 
        { 
            Id = existingDepartment.Id,
            Name = "Updated Cutting Department" 
        };
        
        var handler = new UpdateDepartmentHandler(unitOfWork, mapper);
        var request = new UpdateDepartmentRequest(updateViewModel, _claimsPrincipal);
        
        // Act
        await handler.Handle(request, CancellationToken.None);
        
        // Assert - Get fresh instance from DB to verify update
        var updatedDepartment = await repository.GetFirstOrDefaultAsync(
        predicate: d => d.Id == existingDepartment.Id, 
        trackingType: TrackingType.NoTracking);
    
        Assert.NotNull(updatedDepartment);
        Assert.Equal("Updated Cutting Department", updatedDepartment.Name);
    }
    
    [Fact]
    public async Task DeleteDepartmentHandler_Should_Delete_Department()
    {
        // Arrange
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        
        SeedTestData(unitOfWork);
        
        // Clear the change tracker to ensure no tracked entities
        unitOfWork.DbContext.ChangeTracker.Clear();
        
        var repository = unitOfWork.GetRepository<Domain.Entities.Employees.Department>();
        var existingDepartment = await repository.GetFirstOrDefaultAsync(
            predicate: d => d.Name == "Quality Control",
            trackingType: TrackingType.NoTracking); // Get without tracking
        Assert.NotNull(existingDepartment);
        
        var deleteViewModel = new DeleteDepartmentViewModel { Id = existingDepartment.Id };
        
        var handler = new DeleteDepartmentHandler(unitOfWork);
        var request = new DeleteDepartmentRequest(deleteViewModel, _claimsPrincipal);
        
        // Act
        await handler.Handle(request, CancellationToken.None);
        
        // Assert
        var deletedDepartment = await repository.GetFirstOrDefaultAsync(
            predicate: d => d.Id == existingDepartment.Id,
            trackingType: TrackingType.NoTracking);
        Assert.Null(deletedDepartment); // Department should be deleted
        
        // Verify total count decreased
        var remainingDepartments = repository.GetAll(trackingType: TrackingType.NoTracking);
        Assert.Equal(2, remainingDepartments.Count());
    }
    
    [Fact]
    public Task Department_Should_Allow_Adding_Employees()
    {
        // Arrange - This tests the rich domain model functionality
        var departmentName = "Engineering";
        var department = new Domain.Entities.Employees.Department(departmentName);
        
        // Create a mock employee
        var employee = new Mock<Domain.Entities.Employees.Base.Employee>().Object;
        
        // Act
        department.AddEmployee(employee);
        
        // Assert
        Assert.Single(department.Employees);
        Assert.Contains(employee, department.Employees);
        Assert.Equal(departmentName, department.Name);

        return Task.CompletedTask;
    }
}