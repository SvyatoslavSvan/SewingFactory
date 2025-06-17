using System.Security.Claims;
using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Tests.Common;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Processes.Handlers
{
    public sealed class ProcessHandlersTests
    {
        private readonly ClaimsPrincipal _claimsPrincipal = new(
            new ClaimsIdentity([
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            ])
        );

        private readonly IServiceProvider _serviceProvider = TestHelpers.BuildServices();

        private IUnitOfWork<ApplicationDbContext> GetUow() =>
            _serviceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        private IMapper GetMapper() =>
            _serviceProvider.GetRequiredService<IMapper>();

        private Department SeedDept(IUnitOfWork<ApplicationDbContext> uow) =>
            TestHelpers.SeedDepartment(uow, "Test Dept");

        private List<Process> SeedProcs(IUnitOfWork<ApplicationDbContext> uow, Department dept) =>
            TestHelpers.SeedProcesses(uow, dept);

        [Fact]
        public async Task CreateProcessHandler_Should_Create_And_Return_Process()
        {
            // Arrange
            var uow = GetUow();
            var mapper = GetMapper();
            var dept = SeedDept(uow);

            var handler = new CreateProcessHandler(uow, mapper);
            var model = new CreateProcessViewModel
            {
                Name = "Welding",
                Price = 12m,
                DepartmentId = dept.Id
            };
            var request = new CreateProcessRequest(model, _claimsPrincipal);
            uow.DbContext.ChangeTracker.Clear();
            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Result);
            Assert.Equal("Welding", result.Result.Name);
            Assert.Equal(dept.Id, result.Result.DepartmentViewModel.Id);

            var repo = uow.GetRepository<Process>();
            var saved = await repo.GetFirstOrDefaultAsync(
                predicate: p => p.Name == "Welding",
                trackingType: TrackingType.NoTracking
            );
            Assert.NotNull(saved);
            Assert.Equal(12m, saved.Price.Amount);
        }

        [Fact]
        public async Task GetByIdProcessHandler_Should_Return_Process_When_Found()
        {
            // Arrange
            var uow = GetUow();
            var mapper = GetMapper();
            var dept = SeedDept(uow);
            var procs = SeedProcs(uow, dept);

            var existing = procs.First();
            var handler = new GetByIdProcessHandler(uow.DbContext, mapper);
            var request = new GetByIdProcessRequest(_claimsPrincipal, existing.Id);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Result);
            Assert.Equal(existing.Name, result.Result.Name);
            Assert.Equal(existing.Id, result.Result.Id);
        }

        [Fact]
        public async Task GetAllProcessHandler_Should_Return_All_Processes()
        {
            // Arrange
            var uow = GetUow();
            var mapper = GetMapper();
            var dept = SeedDept(uow);
            SeedProcs(uow, dept);

            var handler = new GetAllProcessHandler(uow, mapper);
            var request = new GetAllProcessRequest(_claimsPrincipal);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            var list = result.Result!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, p => p.Name == "Cutting");
            Assert.Contains(list, p => p.Name == "Sewing");
        }

        [Fact]
        public async Task GetPagedProcessHandler_Should_Return_Paged_Processes()
        {
            // Arrange
            var uow = GetUow();
            var mapper = GetMapper();
            var dept = SeedDept(uow);
            SeedProcs(uow, dept);

            var handler = new GetPagedProcessHandler(uow, mapper);
            var request = new GetPagedProcessRequest(_claimsPrincipal,  0,  1);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Result!.TotalCount);
            Assert.Single(result.Result.Items);
        }

        [Fact]
        public async Task UpdateProcessHandler_Should_Update_Process()
        {
            // Arrange
            var uow = GetUow();
            var mapper = GetMapper();
            var dept = SeedDept(uow);
            var proc = SeedProcs(uow, dept).First();

            // Detach to simulate no-tracking fetch
            uow.DbContext.ChangeTracker.Clear();

            var updateModel = new UpdateProcessViewModel
            {
                Id = proc.Id,
                Name = "Cutting Updated",
                Price = 9m
            };
            var handler = new UpdateProcessHandler(uow, mapper);
            var request = new UpdateProcessRequest(updateModel, _claimsPrincipal);

            // Act
            await handler.Handle(request, CancellationToken.None);

            // Assert
            var repo = uow.GetRepository<Process>();
            var updated = await repo.GetFirstOrDefaultAsync(
                predicate: p => p.Id == proc.Id,
                trackingType: TrackingType.NoTracking
            );
            Assert.NotNull(updated);
            Assert.Equal("Cutting Updated", updated.Name);
            Assert.Equal(9m, updated.Price.Amount);
        }

        [Fact]
        public async Task DeleteProcessHandler_Should_Delete_Process()
        {
            // Arrange
            var uow = GetUow();
            GetMapper();
            var dept = SeedDept(uow);
            var proc = SeedProcs(uow, dept).Last();

            // Clear change tracker as in department tests
            uow.DbContext.ChangeTracker.Clear();

            var deleteModel = new DeleteProcessViewModel { Id = proc.Id };
            var handler = new DeleteProcessHandler(uow);
            var request = new DeleteProcessRequest(deleteModel, _claimsPrincipal);

            // Act
            await handler.Handle(request, CancellationToken.None);

            // Assert
            var repo = uow.GetRepository<Process>();
            var deleted = await repo.GetFirstOrDefaultAsync(
                predicate: p => p.Id == proc.Id,
                trackingType: TrackingType.NoTracking
            );
            Assert.Null(deleted);

            var all = repo.GetAll(TrackingType.NoTracking);
            Assert.Single(all);
        }
    }
}
