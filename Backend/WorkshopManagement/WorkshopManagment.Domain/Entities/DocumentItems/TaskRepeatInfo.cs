namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;

public sealed record TaskRepeatInfo(Guid TaskId, List<EmployeeTaskRepeat> Repeats);