using AutoMapper;
using Calabonga.Microservices.Core;
using Calabonga.Utils.Extensions;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProfileMessages.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProfileMessages;

/// <summary>
///     Mapper Configuration for entity ApplicationUser
/// </summary>
public class ProfilesMapperConfiguration : Profile
{
    /// <inheritdoc />
    public ProfilesMapperConfiguration()
        => CreateMap<ClaimsIdentity, UserProfileViewModel>()
            .ForMember(destinationMember: x => x.Id, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<Guid>(claims, ClaimTypes.Name)))
            .ForMember(destinationMember: x => x.PositionName, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Actor)))
            .ForMember(destinationMember: x => x.FirstName, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.GivenName)))
            .ForMember(destinationMember: x => x.LastName, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Surname)))
            .ForMember(destinationMember: x => x.Roles, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValues<string>(claims, ClaimTypes.Role)))
            .ForMember(destinationMember: x => x.Email, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.Name)))
            .ForMember(destinationMember: x => x.PhoneNumber, memberOptions: o => o.MapFrom(mapExpression: claims => ClaimsHelper.GetValue<string>(claims, ClaimTypes.MobilePhone)));
}