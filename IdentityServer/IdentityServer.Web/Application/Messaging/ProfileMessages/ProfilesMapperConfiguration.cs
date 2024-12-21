using AutoMapper;
using Calabonga.Microservices.Core;
using IdentityServer.Infrastructure;
using IdentityServer.Web.Application.Messaging.ProfileMessages.ViewModels;
using System.Security.Claims;

namespace IdentityServer.Web.Application.Messaging.ProfileMessages;

/// <summary>
///     Mapper Configuration for entity ApplicationUser
/// </summary>
public sealed class ProfilesMapperConfiguration : Profile
{
    /// <inheritdoc />
    public ProfilesMapperConfiguration()
    {
        CreateMap<RegisterViewModel, ApplicationUser>()
            .ForMember(destinationMember: x => x.UserName, memberOptions: o => o.MapFrom(mapExpression: p => p.Email))
            .ForMember(destinationMember: x => x.Email, memberOptions: o => o.MapFrom(mapExpression: p => p.Email))
            .ForMember(destinationMember: x => x.EmailConfirmed, memberOptions: o => o.MapFrom(mapExpression: src => true))
            .ForMember(destinationMember: x => x.FirstName, memberOptions: o => o.MapFrom(mapExpression: p => p.FirstName))
            .ForMember(destinationMember: x => x.LastName, memberOptions: o => o.MapFrom(mapExpression: p => p.LastName))
            .ForMember(destinationMember: x => x.PhoneNumberConfirmed, memberOptions: o => o.MapFrom(mapExpression: src => true))
            .ForMember(destinationMember: x => x.ApplicationUserProfileId, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.ApplicationUserProfile, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.Id, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.NormalizedUserName, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.NormalizedEmail, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.PasswordHash, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.SecurityStamp, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.ConcurrencyStamp, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.PhoneNumber, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.TwoFactorEnabled, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.LockoutEnd, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.LockoutEnabled, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.AccessFailedCount, memberOptions: o => o.Ignore());

        CreateMap<RegisterViewModel, ApplicationUserProfile>()
            .ForMember(destinationMember: x => x.Id, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.Permissions, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.ApplicationUser, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.CreatedAt, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.CreatedBy, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.UpdatedAt, memberOptions: o => o.Ignore())
            .ForMember(destinationMember: x => x.UpdatedBy, memberOptions: o => o.Ignore());

        CreateMap<ClaimsIdentity, UserProfileViewModel>()
            .ForMember(destinationMember: x => x.Id, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<Guid>(claims, ClaimTypes.NameIdentifier)))
            .ForMember(destinationMember: x => x.PositionName, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Actor)))
            .ForMember(destinationMember: x => x.FirstName, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.GivenName)))
            .ForMember(destinationMember: x => x.LastName, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Surname)))
            .ForMember(destinationMember: x => x.Roles, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValues<string>(claims, ClaimTypes.Role)))
            .ForMember(destinationMember: x => x.Email, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Name)))
            .ForMember(destinationMember: x => x.PhoneNumber, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.MobilePhone)));
    }
}