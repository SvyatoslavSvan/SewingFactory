using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.Mapping.Converters
{
    public class EmployeeStubConverter(IUnitOfWork<ApplicationDbContext> unitOfWork) : ITypeConverter<Guid, ProcessBasedEmployee>
    {
        private static readonly Dictionary<Guid, ProcessBasedEmployee> _stubCache = new Dictionary<Guid, ProcessBasedEmployee>();

        public ProcessBasedEmployee Convert(Guid source, ProcessBasedEmployee destination, ResolutionContext context)
        {
            if (_stubCache.TryGetValue(source, out var employee))
            {
                return employee;
            }
            var stub = new ProcessBasedEmployee("stub", "stub", new Department("stub"), 0);
            unitOfWork.DbContext.Entry(stub).Property("Id").CurrentValue = source;
            unitOfWork.DbContext.Entry(stub).State = EntityState.Unchanged;
            _stubCache.Add(stub.Id, stub);
            return stub;
        }
    }
}