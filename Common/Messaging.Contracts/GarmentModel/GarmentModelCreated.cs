namespace SewingFactory.Common.Messaging.Contracts.GarmentModel;

public sealed record GarmentModelCreated(Guid Id, string Name, decimal Price, Guid GarmentCategoryId);