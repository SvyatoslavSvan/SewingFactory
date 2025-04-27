using AutoMapper;
using Calabonga.PagedListCore;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Mapping.Converters;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Mapping.Profiles;

public class PagedListMappingProfile : Profile
{
    public PagedListMappingProfile() => CreateMap(typeof(PagedList<>), typeof(IPagedList<>))
        .ConvertUsing(typeof(PagedListConverter<,>));
}