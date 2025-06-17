namespace SewingFactory.Common.Messaging.Contracts.GarmentModel;

public sealed record GarmentModelUpdated(Guid Id, string Name, decimal Price, Guid GarmentCategoryId);